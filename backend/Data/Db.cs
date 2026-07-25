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

    /// <summary>
    /// True when the server is MariaDB rather than Oracle MySQL. Both answer to DB_PROVIDER=mysql
    /// and speak the same wire protocol, but they disagree about DDL in ways that matter here:
    /// MariaDB supports `CREATE INDEX IF NOT EXISTS` and silently prefixes a lone TEXT key, and
    /// MySQL 8 does neither. Detected from the server's own version banner — MariaDB always
    /// includes "MariaDB" in it — rather than from configuration, which can be wrong.
    /// </summary>
    public bool IsMariaDb { get; private set; }

    private readonly DbConnection _conn;
    private readonly object _gate = new();
    private DbTransaction? _activeTx;
    private readonly string _sqlitePath = "";

    public Db(string path)
    {
        var configuredProvider = Environment.GetEnvironmentVariable("DB_PROVIDER");
        var prov = (configuredProvider ?? "sqlite").Trim().ToLowerInvariant();
        if (configuredProvider is not null && prov is not ("sqlite" or "mysql" or "mariadb"))
            throw new InvalidOperationException(
                $"Unknown DB_PROVIDER '{configuredProvider}'. Expected sqlite, mysql, or mariadb; refusing fallback.");
        if (prov is "mysql" or "mariadb")
        {
            Provider = Kind.MySql;
            _conn = new MySqlConnection(MySqlConnectionString());
        }
        else
        {
            Provider = Kind.Sqlite;
            _sqlitePath = path;
            _conn = new SqliteConnection($"Data Source={path};Cache=Shared");
        }
        OpenWithRetry();
        if (Provider == Kind.MySql)
            IsMariaDb = (_conn.ServerVersion ?? "").Contains("MariaDB", StringComparison.OrdinalIgnoreCase);
        if (Provider == Kind.Sqlite)
        {
            Execute("PRAGMA journal_mode=WAL");
            Execute("PRAGMA foreign_keys=ON");
        }
        else
        {
            // Lenient mode for parity with SQLite's dynamic typing: coerce rather than error on a
            // type/length mismatch, so behaviour matches the SQLite provider exactly.
            // PIPES_AS_CONCAT makes `||` a string concatenation operator (as on SQLite) instead of logical OR;
            // strict flags stay off so existing INSERT behaviour is unchanged across both providers.
            Execute("SET SESSION sql_mode='PIPES_AS_CONCAT'");
        }
    }

    /// <summary>Open the connection, retrying transient failures. A managed MySQL can be briefly
    /// unavailable at boot (deploy, failover); retry with backoff rather than crash the process.
    /// SQLite opens locally and effectively never transiently fails, so it opens once.</summary>
    void OpenWithRetry()
    {
        if (Provider == Kind.Sqlite) { _conn.Open(); return; }
        var attempts = int.TryParse(Environment.GetEnvironmentVariable("MYSQL_CONNECT_RETRIES"), out var n) ? Math.Clamp(n, 1, 20) : 6;
        for (var i = 1; ; i++)
        {
            try { _conn.Open(); return; }
            catch (Exception ex) when (i < attempts)
            {
                var wait = Math.Min(1000 * i, 8000);
                Console.Error.WriteLine($"[db] MySQL connect attempt {i}/{attempts} failed ({ex.Message}); retrying in {wait}ms");
                Thread.Sleep(wait);
            }
        }
    }

    static string MySqlConnectionString()
    {
        var cs = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        if (!string.IsNullOrWhiteSpace(cs)) return cs;
        string E(string k, string def) => Environment.GetEnvironmentVariable(k) is { Length: > 0 } v ? v : def;
        uint U(string k, uint def) => uint.TryParse(E(k, ""), out var v) ? v : def;
        var b = new MySqlConnectionStringBuilder
        {
            Server = E("MYSQL_HOST", "127.0.0.1"),
            Port = U("MYSQL_PORT", 3306),
            Database = E("MYSQL_DATABASE", "pci"),
            UserID = E("MYSQL_USER", "root"),
            Password = Environment.GetEnvironmentVariable("MYSQL_PASSWORD") ?? "",
            CharacterSet = "utf8mb4",
            // strict types off so an empty string bound to a numeric column coerces like SQLite,
            // matching the app's loose-typing expectations.
            AllowUserVariables = true,
            ConnectionTimeout = U("MYSQL_CONNECT_TIMEOUT", 15),          // seconds to establish a connection
            DefaultCommandTimeout = U("MYSQL_COMMAND_TIMEOUT", 30),      // seconds per command
            Pooling = true,
            MinimumPoolSize = U("MYSQL_POOL_MIN", 0),
            MaximumPoolSize = U("MYSQL_POOL_MAX", 20),
        };
        // SSL: default Preferred; MYSQL_SSL=false disables (local/dev), MYSQL_SSL=required enforces (managed prod).
        var ssl = Environment.GetEnvironmentVariable("MYSQL_SSL")?.Trim().ToLowerInvariant();
        if (ssl == "false") b.SslMode = MySqlSslMode.Disabled;
        else if (ssl is "required" or "true") b.SslMode = MySqlSslMode.Required;
        return b.ConnectionString;
    }

    // ---- SQLite→MySQL SQL translation (no-op for SQLite) ----
    private static readonly System.Text.RegularExpressions.Regex RxDatetimeMod =
        new(@"datetime\('now',\s*'([+-]?\d+)\s+(year|years|month|months|day|days|hour|hours|minute|minutes|second|seconds)'\)",
            System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

    /// <summary>The DATE-only sibling of the above. It was missing, so `date('now','-1 day')` — used
    /// by the world admin's calendar query — passed through untranslated and MySQL rejected it:
    /// DATE() takes one argument there. Same shape, day-precision output.</summary>
    private static readonly System.Text.RegularExpressions.Regex RxDateMod =
        new(@"date\('now',\s*'([+-]?\d+)\s+(year|years|month|months|day|days|hour|hours|minute|minutes|second|seconds)'\)",
            System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);

    private static string Unit(string u) => u.TrimEnd('s').ToUpperInvariant();  // minutes→MINUTE, day→DAY

    private string Translate(string sql) =>
        Provider == Kind.Sqlite ? sql : TranslateFor(sql, IsMariaDb);

    /// <summary>
    /// The SQLite→MySQL/MariaDB rewrite, as a pure function so the dialect rules can be tested
    /// without a database server. `mariaDb` selects between the two engines wherever they differ.
    /// </summary>
    public static string TranslateFor(string sql, bool mariaDb)
    {
        var s = sql;
        // datetime('now','+N unit') → DATE_FORMAT(DATE_ADD(UTC_TIMESTAMP(), INTERVAL N UNIT),'…')
        s = RxDatetimeMod.Replace(s, m =>
            $"DATE_FORMAT(DATE_ADD(UTC_TIMESTAMP(), INTERVAL {m.Groups[1].Value} {Unit(m.Groups[2].Value)}),'%Y-%m-%d %H:%i:%s')");
        s = RxDateMod.Replace(s, m =>
            $"DATE_FORMAT(DATE_ADD(UTC_TIMESTAMP(), INTERVAL {m.Groups[1].Value} {Unit(m.Groups[2].Value)}),'%Y-%m-%d')");
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
        // Oracle MySQL permits defaults on BLOB/TEXT only when written as expressions. A quoted
        // literal therefore needs parentheses (`TEXT DEFAULT ('active')`); MariaDB accepts the
        // SQLite spelling directly. This applies to runtime installers as well as the base schema.
        if (!mariaDb)
            s = System.Text.RegularExpressions.Regex.Replace(
                s,
                @"\bTEXT\b(\s+NOT\s+NULL)?\s+DEFAULT\s+('(?:''|[^'])*')",
                "TEXT$1 DEFAULT ($2)",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        // CREATE INDEX IF NOT EXISTS: valid on MariaDB, a SYNTAX ERROR on MySQL 8. Strip it ONLY for
        // MySQL, and let Exec() absorb the duplicate-key error instead — same idempotence, both
        // engines. On MariaDB the clause is load-bearing: the core schema declares some indexes both
        // inline and again as a guarded CREATE INDEX, and removing the guard turns a skip into a real
        // creation attempt that fails on key length. (CREATE TABLE IF NOT EXISTS is fine on both.)
        if (!mariaDb)
            s = System.Text.RegularExpressions.Regex.Replace(s,
                @"(CREATE\s+(UNIQUE\s+)?INDEX\s+)IF\s+NOT\s+EXISTS\s+", "$1",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
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
            if (Provider == Kind.MySql && !IsMariaDb)
                sqlScript = RemoveExistingMySqlIndexes(sqlScript);
            var sql = Translate(sqlScript);
            try
            {
                using var cmd = NewCmd();
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (MySqlConnector.MySqlException e) when (Provider == Kind.MySql && IsIndexDdl(sql))
            {
                // ── MySQL 8 vs MariaDB: the two engines disagree about CREATE INDEX, and the schema
                // is written once in the SQLite dialect for both. MariaDB hides both differences
                // (it accepts IF NOT EXISTS and silently prefixes a TEXT key); MySQL 8 rejects them,
                // and because schema installation is wrapped in a catch-and-log, the FIRST rejection
                // would abort the rest of the install and leave the product half-created. So the two
                // known divergences are handled here, explicitly, rather than left to chance.
                switch (e.ErrorCode)
                {
                    // 1061 duplicate key name — the index already exists. This IS the "IF NOT EXISTS"
                    // semantics the statement asked for, which MySQL 8 cannot express in syntax.
                    case MySqlErrorCode.DuplicateKeyName:
                        return;
                    // 1170 BLOB/TEXT column used in key specification without a key length. MariaDB
                    // adds a prefix itself; MySQL 8 refuses. Retry with an explicit prefix that fits
                    // inside the 3072-byte InnoDB limit under utf8mb4 (191 chars × 4 = 764 bytes).
                    case MySqlErrorCode.BlobKeyWithoutLength:
                        try
                        {
                            using var retry = NewCmd();
                            retry.CommandText = PrefixIndexedColumns(sql);
                            retry.ExecuteNonQuery();
                            return;
                        }
                        catch (MySqlConnector.MySqlException r) when (r.ErrorCode == MySqlErrorCode.DuplicateKeyName) { return; }
                    default: throw;
                }
            }
        }
    }

    /// <summary>
    /// Oracle MySQL 8 does not support CREATE INDEX IF NOT EXISTS. The base schema is a multi-statement
    /// script, so the normal single-index duplicate handler cannot recover once a duplicate aborts the
    /// batch. Remove only index statements whose names already exist; missing indexes remain in the
    /// script and TranslateFor strips their unsupported IF NOT EXISTS clause.
    /// </summary>
    string RemoveExistingMySqlIndexes(string script)
    {
        var rx = new System.Text.RegularExpressions.Regex(
            @"CREATE\s+(?:UNIQUE\s+)?INDEX\s+IF\s+NOT\s+EXISTS\s+`?([A-Za-z0-9_]+)`?\s+ON\s+`?([A-Za-z0-9_]+)`?[^;]*;",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        return rx.Replace(script, match =>
        {
            using var cmd = NewCmd();
            cmd.CommandText = @"SELECT COUNT(*) FROM information_schema.statistics
                WHERE table_schema=DATABASE() AND index_name=@indexName AND table_name=@tableName";
            var p1 = cmd.CreateParameter(); p1.ParameterName = "@indexName"; p1.Value = match.Groups[1].Value;
            var p2 = cmd.CreateParameter(); p2.ParameterName = "@tableName"; p2.Value = match.Groups[2].Value;
            cmd.Parameters.Add(p1); cmd.Parameters.Add(p2);
            return Convert.ToInt64(cmd.ExecuteScalar()) > 0 ? "" : match.Value;
        });
    }

    static bool IsIndexDdl(string sql) =>
        sql.TrimStart().StartsWith("CREATE", StringComparison.OrdinalIgnoreCase) &&
        sql.Contains("INDEX", StringComparison.OrdinalIgnoreCase);

    /// <summary>Rewrite `CREATE INDEX x ON t(a, b)` as `… (a(191), b(191))`. Applied only after MySQL
    /// has told us a column in this index is a BLOB/TEXT that needs a length; a prefix on a column
    /// that does not need one is harmless and indexes the same leading characters MariaDB would.</summary>
    static string PrefixIndexedColumns(string sql)
    {
        var open = sql.LastIndexOf('(');
        var close = sql.LastIndexOf(')');
        if (open < 0 || close < open) return sql;
        var cols = sql[(open + 1)..close].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var rewritten = cols.Select(c => c.Contains('(') ? c : c + "(191)");
        return sql[..(open + 1)] + string.Join(", ", rewritten) + sql[close..];
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

    /// <summary>Run <paramref name="body"/> under a cross-instance schema-migration lock so that when several
    /// app instances boot at once (rolling deploy, scale-out) only ONE runs DDL at a time and the others wait —
    /// preventing DDL races and duplicate-key inserts into the migration ledger. MySQL uses a connection-scoped
    /// advisory lock (GET_LOCK); SQLite uses a best-effort lock file next to the database (single-host).</summary>
    public void WithMigrationLock(Action body)
    {
        if (Provider == Kind.MySql)
        {
            var got = Scalar<long>("SELECT GET_LOCK('pci_schema_migrate', 120)");
            if (got != 1) throw new Exception("could not acquire the MySQL schema-migration lock (GET_LOCK timed out)");
            try { body(); }
            finally { try { Execute("DO RELEASE_LOCK('pci_schema_migrate')"); } catch { } }
            return;
        }
        var lockPath = (_sqlitePath is { Length: > 0 } p && p != ":memory:") ? p + ".migrate.lock" : null;
        if (lockPath is null) { body(); return; }   // in-memory db: nothing to coordinate across processes
        FileStream? fl = null;
        var deadline = DateTime.UtcNow.AddSeconds(120);
        while (fl is null)
        {
            try { fl = new FileStream(lockPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None); }
            catch (IOException) { if (DateTime.UtcNow > deadline) { body(); return; } Thread.Sleep(200); }
        }
        try { body(); }
        finally { fl.Dispose(); }
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
