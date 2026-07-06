using Microsoft.Data.Sqlite;

namespace PCI.Backend.Data;

/// <summary>
/// Thin data-access layer over Microsoft.Data.Sqlite that mirrors the ergonomics of the Node backend's
/// better-sqlite3 usage (Query/QueryOne/Execute/Scalar) so the ported endpoints read almost identically.
/// A single shared connection (SQLite serialises writes; WAL is on) keeps parity with the Node process.
/// </summary>
public sealed class Db
{
    private readonly SqliteConnection _conn;
    private readonly object _gate = new();

    public Db(string path)
    {
        _conn = new SqliteConnection($"Data Source={path};Cache=Shared");
        _conn.Open();
        Execute("PRAGMA journal_mode=WAL");
        Execute("PRAGMA foreign_keys=ON");
    }

    // parameters passed positionally as ?, matching the Node prepared-statement style
    // Microsoft.Data.Sqlite requires command.Transaction == connection.Transaction.
    // Creating a command via this helper associates it with the currently-active
    // transaction (or null when none), so queries work both inside and outside Transaction().
    private SqliteCommand NewCmd()
    {
        var cmd = _conn.CreateCommand();
        cmd.Transaction = _conn.Transaction;
        return cmd;
    }

    private static void Bind(SqliteCommand cmd, object?[] args)
    {
        // Microsoft.Data.Sqlite uses $p0,$p1... — rewrite each '?' placeholder in order
        int i = 0;
        var text = cmd.CommandText;
        var sb = new System.Text.StringBuilder(text.Length + 8);
        foreach (var ch in text)
        {
            if (ch == '?') { sb.Append("$p").Append(i); i++; }
            else sb.Append(ch);
        }
        cmd.CommandText = sb.ToString();
        for (int j = 0; j < args.Length; j++)
            cmd.Parameters.AddWithValue($"$p{j}", args[j] ?? DBNull.Value);
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
            cmd.CommandText = sql + "; SELECT last_insert_rowid();";
            Bind(cmd, args);
            var v = cmd.ExecuteScalar();
            return v is long l ? l : Convert.ToInt64(v);
        }
    }

    /// <summary>Runs an INSERT and returns (rowid, changes) from ONE command — safe for
    /// INSERT OR IGNORE idempotency checks (no interleaving on the shared connection).</summary>
    public (long id, long changes) ExecuteWithChanges(string sql, params object?[] args)
    {
        lock (_gate)
        {
            using var cmd = NewCmd();
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

    /// <summary>One row as a case-insensitive dictionary, or null.</summary>
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
            cmd.CommandText = sqlScript;
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>Runs an action inside an IMMEDIATE transaction on the shared connection.
    /// Commits on success, rolls back on any exception. Used for atomic webhook settlement.</summary>
    public void Transaction(Action body)
    {
        lock (_gate)
        {
            using var tx = _conn.BeginTransaction();
            try { body(); tx.Commit(); }
            catch { try { tx.Rollback(); } catch { } throw; }
        }
    }

    public HashSet<string> Columns(string table)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var row in Query($"PRAGMA table_info({table})"))
            if (row["name"] is string n) set.Add(n);
        return set;
    }
}
