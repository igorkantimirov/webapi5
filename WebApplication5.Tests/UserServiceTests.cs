using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using WebApplication5.Controllers;
using WebApplication5.Helpers;
using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services;

namespace WebApplication5.Tests;

public class Tests
{
    private UserService _userService;
    private IUserRepository _userRepository;

    [SetUp]
    public void Setup()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _userRepository.FindSingleByIdAsync("testId")
            .Returns(new User()
            {
                _id = "testId",
                Email = "test@example.com",
                Username = "test"
            });

        _userService = new UserService(new PasswordHelper(), _userRepository);
    }

    [Test]
    public async Task FindByIdAsync_WhenCalled_ReturnsUser()
    {
        var testUser = await _userService.FindByIdAsync("testId");
        Assert.AreEqual("testId", testUser._id);
        Assert.AreEqual("test@example.com", testUser.Email);
        Assert.AreEqual("test", testUser.Username);
    }

    [Test]
    public async Task CreateAsync_WhenCalled_CreatesUser()
    {
        var createdUser = await _userService.CreateAsync(new CreateUserReqDto
        {
            Username = "test2",
            Email = "test2@example.com",
            Password = "test2"
        });

        await _userRepository.Received(1).InsertOneAsync(Arg.Any<User>());
    }

    [Test]
    public async Task CreateAsync_WhenUserAlreadyExists_DoesntCreateUser()
    {
        var createUserReqDto = new CreateUserReqDto
        {
            Username = "test2",
            Email = "test2@example.com",
            Password = "test2"
        };
        
        await _userService.CreateAsync(createUserReqDto);
        _userRepository.FindSingleAsync(createUserReqDto.Email, createUserReqDto.Username).Returns(
            new User());
        Assert.ThrowsAsync<AuthenticationException>(async () => await _userService.CreateAsync(createUserReqDto));
        await _userRepository.Received(1).InsertOneAsync(Arg.Any<User>());
    }
}