using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using WebApi.Test.Helpers;
using WebApplication5.Controllers;
using WebApplication5.Data;
using WebApplication5.Helpers;
using WebApplication5.Services;

namespace WebApplication5.Tests;

public class UserServiceTest : IDisposable
{
    private IUserService _service;
    private AppDbContext _db;
    private PasswordHelper _passwordHelper;

    public UserServiceTest()
    {
        _passwordHelper = new PasswordHelper();
        _db = new DataHelper().CreateDbContext(_passwordHelper);
        _service = new UserService(_db, _passwordHelper);
    }
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task RegisterAsync_WhenCalled_CreatesUser()
    {
        var registerAsyncReqDto = new RegisterAsyncReqDto()
        {
            Email = "test@example.com",
            Password = "123asd456cvb",
            Username = "testUsername"
        };
        
        await _service.RegisterAsync(registerAsyncReqDto);

        var user = await _db.Users.FirstOrDefaultAsync(x => x.Username == registerAsyncReqDto.Username);
        if (user == null)
            throw new AssertionException("User have not been created");
        
        Assert.AreEqual(registerAsyncReqDto.Email, user.Email);
        Assert.AreEqual(registerAsyncReqDto.Username, user.Username);
        Assert.AreEqual(_passwordHelper.GetHashedPassword(registerAsyncReqDto.Email), user.PasswordHashed);
    }

    [Test]
    public async Task AuthenticateAsync_WhenCalled_AuthenticatesUser()
    {
        var authenticateAsyncReqDto = new AuthenticateAsyncReqDto()
        {
            Username = "testUserFixture",
            Password = "testPassword"
        };
        var handler = new JwtSecurityTokenHandler();
        
        var user = await _service.AuthenticateAsync(authenticateAsyncReqDto);
        var claimMaybe = handler.ReadJwtToken(user.Token).Claims.FirstOrDefault();
        if (claimMaybe == null)
            throw new AssertionException("Token is incorrect");

        var userId = claimMaybe.Value;

        Assert.AreEqual(authenticateAsyncReqDto.Username, user.Username);
        Assert.AreEqual("ca761232-ed42-11ce-bacd-00aa0057b223", userId);
    }

    [Test]
    public async Task GetUserByIdAsync_WhenCalled_ReturnsUser()
    {
        var user = await _service.GetUserByIdAsync(new Guid("CA761232-ED42-11CE-BACD-00AA0057B223"));

        Assert.AreEqual("testUserFixture", user.Username);
    }
    
    public void Dispose()
    {
        _db.Dispose();
    }
}