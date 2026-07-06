using System.Data.Common;
using Microsoft.Data.Sqlite;
using MySqlConnector;

namespace PCI.Backend.Data;

/// <summary>
/// Dual-provider data-access layer. The app is written against SQLite dialect SQL (the source of
/// truth); when DB_PROVIDER=mysql this class translates that SQL to MySQL/MariaDB at runtime and
/// talks to a MySqlConnection instead. Everything else — the ~430 db.Query/Execute/Scalar call sites,
/// the endpoints, the migrations — is unchanged and provider-agnostic.
///
/// Key design choice: datetimes are handled as STRINGS in the SQLite format "YYYY-MM-DD HH:MM:SS" on
/// BOTH providers (MySQL datetime columns are VARCHAR; datetime('now',...) translates to a
/// DATE_FORMAT(UTC_TIMESTAMP(),...) expression producing the same string). So all of the app's
/// string-based date parsing/compare logic (H.JsMillis / H.IsPast / H.After) is identical everywhere.
///
/// A single shared connection (guarded by _gate) keeps parity with the original SQLite design.
/// </summary>
public sealed class Db
{
    public enum Kind { Sqlite, MySql }
    public Kind Provider { get; }

    private readonly DbConnection _conn;
    private readonly object _gate = new();
    private DbTransaction? _activeTx;

    public Db(string path)
    {
        var prov = (Environment.GetEnvironmentVariable("DB_PROVIDER") ?? "sqlite").Trim().ToLowerInvariant();
        if (prov is "mysql" or "mariadb")
        {
            Provider = Kind.MySql;
            _conn = new MySqlConnection(MySqlConnectionString());
        }
        else
        {
            Provider = Kind.Sqlite;
            _conn = new SqliteConnection($"Data Source={path};Cache=Shared");
        }
        _conn.Open();
        if (Provider == Kind.Sqlite)
        {
            Execute("PRAGMA journal_mode=WAL");
            Execute("PRAGMA foreign_keys=ON");
        }
        else
        {
            // Lenient mode for parity with SQLite's dynamic typing: coerce rather than error on a
            // type/length mismatch, so behaviour matches the SQLite provider exactly.
            Execute("SET SESSION sql_mode=''");
        }
    }

    static string MySqlConnectionString()
    {
        var cs = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(cs)) return cs;
        string E(string k, string def) => Environment.GetEnvironmentVariable(k) is { Length: > 0 } v ? v : def;
        var b = new MySqlConnectionStringBuilder
        {
            Server = E("MYSQL_HOST", "127.0.0.1"),
            Port = uint.TryParse(E("MYSQL_PORT", "3306"), out var p) ? p : 3306,
            Database = E("MYSQL_DATABASE", "pci"),
            UserID = E("MYSQL_USER", "root"),
            Password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "",
            CharacterSet = "utf8mb4",
            // strict types off so an empty string bound to a numeric column coerces like SQLite,
            // matching the app's loose-typing expectations.
            AllowUserVariables = true,
            ConnectionTimeout = 15,
        };
        if (Environment.GetEnvironmentVariable("MYSQL_SSL") is "false") b.SslMode = MySqlSslMode.Disabled;
        return b.ConnectionString;
    }

    // ---- SQLite→MySQL SQL translation (no-op for SQLite) ----
    private static readonly System.Text.RegularExpressions.Regex RxDatetimeMod =
        new(@"datetime\('now',\s*'([+-]?\d+)\s+(year|years|month|months|day|days|hour|hours|minute|minutes|second|seconds)'\)",
            System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

    private static string Unit(string u) => u.TrimEnd('s').ToUpperInvariant();  // minutes→MINUTE, day→DAY

    private string Translate(string sql)
    {
        if (Provider == Kind.Sqlite) return sql;
        var s = sql;
        // datetime('now','+N unit') → DATE_FORMAT(DATE_ADD(UTC_TIMESTAMP(), INTERVAL N UNIT),'…')
        s = RxDatetimeMod.Replace(s, m =>
            $"DATE_FORMAT(DATE_ADD(UTC_TIMESTAMP(), INTERVAL {m.Groups[1].Value} {Unit(m.Groups[2].Value)}),'%Y-%m-%d %H:%i:%s')");
        // date('now','start of month') → first day of the current UTC month at midnight
        s = s.Replace("date('now','start of month')", "CONCAT(DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m'),'-01')");
        s = s.Replace("datetime('now','start of month')", "CONCAT(DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m'),'-01 00:00:00')");
        // datetime('now') / date('now')
        s = s.Replace("datetime('now')", "DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m-%d %H:%i:%s')");
        s = s.Replace("date('now')", "DATE_FORMAT(UTC_TIMESTAMP(),'%Y-%m-%d')");
        // strftime('<fmt>', x) → DATE_FORMAT(x,'<fmt>') with SQLite→MySQL format-code mapping (%M→%i, %S→%s)
        s = System.Text.RegularExpressions.Regex.Replace(s, @"strftime\('([^']+)',\s*([^)]+)\)",
            m => $"DATE_FORMAT({m.Groups[2].Value},'{m.Groups[1].Value.Replace("%M", "%i").Replace("%S", "%s")}')");
        // the two proctoring julianday expressions (fixed shapes) → TIMESTAMPDIFF seconds
        s = s.Replace("CAST(MAX(0,(julianday(a.started_at)+a.duration_minutes/1440.0-julianday('now'))*86400) AS INT)",
                      "GREATEST(0, TIMESTAMPDIFF(SECOND, UTC_TIMESTAMP(), DATE_ADD(a.started_at, INTERVAL a.duration_minutes MINUTE)))");
        s = s.Replace("CAST((julianday('now')-julianday(COALESCE(a.last_heartbeat_at,a.started_at)))*86400 AS INT)",
                      "TIMESTAMPDIFF(SECOND, COALESCE(a.last_heartbeat_at,a.started_at), UTC_TIMESTAMP())");
        // generic julianday(EXPR)-julianday('now') day-difference → DATEDIFF
        s = System.Text.RegularExpressions.Regex.Replace(s, @"CAST\(julianday\(([^)]+)\)-julianday\('now'\) AS INT\)",
            "DATEDIFF($1, UTC_TIMESTAMP())");
        // idempotent inserts / upsert
        s = s.Replace("INSERT OR IGNORE", "INSERT IGNORE");
        s = s.Replace("INSERT OR REPLACE", "REPLACE");
        s = s.Replace("ON CONFLICT(skey) DO UPDATE SET svalue=excluded.svalue", "ON DUPLICATE KEY UPDATE svalue=VALUES(svalue)");
        // scalar functions
        s = s.Replace("last_insert_rowid()", "LAST_INSERT_ID()");
        s = s.Replace("changes()", "ROW_COUNT()");
        // DDL (Migrate.cs idempotent statements written in SQLite dialect)
        s = s.Replace("INTEGER PRIMARY KEY AUTOINCREMENT", "BIGINT PRIMARY KEY AUTO_INCREMENT");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\bINTEGER\b", "BIGINT");
        // partial unique index → plain (MySQL exempts NULLs from UNIQUE already)
        s = System.Text.RegularExpressions.Regex.Replace(s, @"(CREATE UNIQUE INDEX[^;]*?)\s+WHERE[^;]*", "$1");
        // remaining CAST(... AS INT) → SIGNED
        s = s.Replace(" AS INT)", " AS SIGNED)");
        return s;
    }

    private DbCommand NewCmd()
    {
        var cmd = _conn.CreateCommand();
        cmd.Transaction = _activeTx;
        return cmd;
    }

    private void Bind(DbCommand cmd, object?[] args)
    {
        // rewrite each positional '?' to a named @p{n}; both providers accept the @-prefixed form.
        int i = 0;
        var text = cmd.CommandText;
        var sb = new System.Text.StringBuilder(text.Length + 8);
        foreach (var ch in text)
        {
            if (ch == '?') { sb.Append("@p").Append(i); i++; }
            else sb.Append(ch);
        }
        cmd.CommandText = Translate(sb.ToString());
        for (int j = 0; j < args.Length; j++)
        {
            var prm = cmd.CreateParameter();
            prm.ParameterName = $"@p{j}";
            prm.Value = args[j] ?? DBNull.Value;
            cmd.Parameters.Add(prm);
        }
    }

    public int Execute(string sql, params object?[] args)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
            cmd.CommandText = sql;
            Bind(cmd, args);
            return cmd.ExecuteNonQuery();
        }
    }

    public long ExecuteReturningId(string sql, params object?[] args)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
            if (Provider == Kind.MySql)
            {
                cmd.CommandText = sql;
                Bind(cmd, args);
                cmd.ExecuteNonQuery();
                return ((MySqlCommand)cmd).LastInsertedId;
            }
            cmd.CommandText = sql + "; SELECT last_insert_rowid();";
            Bind(cmd, args);
            var v = cmd.ExecuteScalar();
            return v is long l ? l : Convert.ToInt64(v);
        }
    }

    /// <summary>Runs an INSERT and returns (rowid, changes). On MySQL, LastInsertedId + rows-affected
    /// give the same semantics as SQLite's last_insert_rowid()/changes() — for INSERT IGNORE that was
    /// ignored, both are 0, which is exactly what the idempotency gates rely on.</summary>
    public (long id, long changes) ExecuteWithChanges(string sql, params object?[] args)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
            if (Provider == Kind.MySql)
            {
                cmd.CommandText = sql;
                Bind(cmd, args);
                var affected = cmd.ExecuteNonQuery();
                var id = ((MySqlCommand)cmd).LastInsertedId;
                return (id, affected);
            }
            cmd.CommandText = sql + "; SELECT last_insert_rowid(), changes();";
            Bind(cmd, args);
            using var r = cmd.ExecuteReader();
            if (r.Read())
            {
                var id = r.IsDBNull(0) ? 0 : Convert.ToInt64(r.GetValue(0));
                var ch = r.IsDBNull(1) ? 0 : Convert.ToInt64(r.GetValue(1));
                return (id, ch);
            }
            return (0, 0);
        }
    }

    public T? Scalar<T>(string sql, params object?[] args)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
            cmd.CommandText = sql;
            Bind(cmd, args);
            var v = cmd.ExecuteScalar();
            if (v is null || v is DBNull) return default;
            return (T)Convert.ChangeType(v, typeof(T));
        }
    }

    public Dictionary<string, object?>? QueryOne(string sql, params object?[] args)
    {
        var rows = Query(sql, args);
        return rows.Count > 0 ? rows[0] : null;
    }

    public List<Dictionary<string, object?>> Query(string sql, params object?[] args)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
            cmd.CommandText = sql;
            Bind(cmd, args);
            using var r = cmd.ExecuteReader();
            var list = new List<Dictionary<string, object?>>();
            while (r.Read())
            {
                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < r.FieldCount; i++)
                    row[r.GetName(i)] = r.IsDBNull(i) ? null : r.GetValue(i);
                list.Add(row);
            }
            return list;
        }
    }

    public void Exec(string sqlScript)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
            cmd.CommandText = Translate(sqlScript);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>Runs an action inside a transaction on the shared connection. Commits on success,
    /// rolls back on any exception. Monitor is re-entrant so nested Execute/Query see _activeTx.</summary>
    public void Transaction(Action body)
    {
        lock (_gate)
        {
            using var tx = _conn.BeginTransaction();
            _activeTx = tx;
            try { body(); tx.Commit(); }
            catch { try { tx.Rollback(); } catch { } throw; }
            finally { _activeTx = null; }
        }
    }

    public HashSet<string> Columns(string table)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (Provider == Kind.MySql)
        {
            foreach (var row in Query($"SHOW COLUMNS FROM `{table}`"))
                if (row.TryGetValue("Field", out var f) && f is string n) set.Add(n);
        }
        else
        {
            foreach (var row in Query($"PRAGMA table_info({table})"))
                if (row["name"] is string n) set.Add(n);
        }
        return set;
    }
}
