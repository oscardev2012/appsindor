using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace SafeManagement.Data;

public sealed class SafeSql
{
    private readonly string _connectionString;
    public SafeSql(string connectionString) => _connectionString = connectionString;

    public SqlConnection Open()
    {
        var db = new SqlConnection(_connectionString);
        db.Open();
        return db;
    }
}

public static class SafeSqlExtensions
{
    public static SqlCommand Command(this SqlConnection db, string sql, SqlTransaction? tx = null)
    {
        var cmd = db.CreateCommand();
        cmd.CommandText = sql;
        cmd.Transaction = tx;
        return cmd;
    }

    public static SqlCommand With(this SqlCommand cmd, string name, object? value)
    {
        cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
        return cmd;
    }

    public static string Text(this SqlDataReader r, int i) => r.IsDBNull(i) ? "" : r.GetString(i);

    public static string Iso(this SqlDataReader r, int i)
        => r.IsDBNull(i) ? "" : DateTime.SpecifyKind(r.GetDateTime(i), DateTimeKind.Utc).ToString("o");

    public static string? IsoOrNull(this SqlDataReader r, int i) => r.IsDBNull(i) ? null : r.Iso(i);

    public static double Number(this SqlDataReader r, int i) => r.IsDBNull(i) ? 0 : (double)r.GetDecimal(i);

    public static double? NumberOrNull(this SqlDataReader r, int i) => r.IsDBNull(i) ? null : (double)r.GetDecimal(i);
}

public sealed class SafeDatabaseErrorFilter : IExceptionFilter
{
    private readonly ILogger<SafeDatabaseErrorFilter> _log;
    public SafeDatabaseErrorFilter(ILogger<SafeDatabaseErrorFilter> log) => _log = log;

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not SqlException ex) return;
        _log.LogError(ex, "IndorDB error on {Path}", context.HttpContext.Request.Path);
        var missingTable = ex.Number == 208;
        context.Result = new ObjectResult(new
        {
            error = missingTable
                ? "IndorDB is missing the SAFE tables. Run sql/indordb-app-tables.sql."
                : "IndorDB is not available. Try again in a moment.",
            errorEs = missingTable
                ? "A IndorDB le faltan las tablas de SAFE. Corre sql/indordb-app-tables.sql."
                : "IndorDB no está disponible. Intenta de nuevo en un momento."
        })
        { StatusCode = 503 };
        context.ExceptionHandled = true;
    }
}
