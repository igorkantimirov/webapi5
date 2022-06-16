using System;
using WebApplication5.Data;
using WebApplication5.Helpers;
using WebApplication5.Models;
using WebApplication5.Tests.Helpers;

namespace WebApi.Test.Helpers;

public class DataHelper
{
    /// <summary>
    /// Creates the "in memory" database context and seeds it with fixtures.
    /// </summary>
    /// <returns>The database context seeded with fixtures.</returns>
    public AppDbContext CreateDbContext(IPasswordHelper passwordHelper)
    {
        var dbContext = new SqliteInMemoryAppDbContext(null);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.Add(new User
        {
            Id = new Guid("CA761232-ED42-11CE-BACD-00AA0057B223"),
            Username = "testUserFixture",
            Email = "testUserFixture@example.com",
            CreatedAt = default,
            UpdatedAt = null,
            LastLoginAt = null,
            PasswordHashed = passwordHelper.GetHashedPassword("testPassword")
        });
        dbContext.SaveChanges();
        
        return dbContext;
    }
}