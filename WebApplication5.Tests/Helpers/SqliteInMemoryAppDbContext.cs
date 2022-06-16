using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplication5.Data;

namespace WebApplication5.Tests.Helpers;

/// <summary>
/// The SQLite in-memory database.
/// </summary>
/// <seealso cref="AppDbContext" />
public class SqliteInMemoryAppDbContext : AppDbContext
{
    public SqliteInMemoryAppDbContext(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        options.UseSqlite(connection);
    }
}