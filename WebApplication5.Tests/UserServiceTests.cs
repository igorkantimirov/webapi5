using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using WebApplication5.Helpers;
using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services;

namespace WebApplication5.Tests;

public class Tests
{
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        var userRepository = Substitute.For<IUserRepository>();
        userRepository.FindSingleByIdAsync("testId")
            .Returns(new User()
            {
                _id = "testId",
                Email = "test@example.com",
                Username = "test"
            });
        
        _userService = new UserService(new PasswordHelper(), userRepository);
    }

    [Test]
    public async Task FindByIdAsync_WhenCalled_ReturnsUser()
    {
        var testUser = await _userService.FindByIdAsync("testId");
        Assert.AreEqual("testId", testUser._id);
        Assert.AreEqual("test@example.com", testUser.Email);
        Assert.AreEqual("test", testUser.Username);
    }
}