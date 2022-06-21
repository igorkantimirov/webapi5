using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Helpers;
using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IAuthHelper _authHelper;

    public UserController(ILogger<UserController> logger, IUserService userService, IAuthHelper authHelper)
    {
        _logger = logger;
        _userService = userService;
        _authHelper = authHelper;
    }

    [AllowAnonymous]
    [HttpPost("create")]
    [ActionName(nameof(Create))]
    public async Task<ActionResult> Create([FromBody] CreateUserReqDto userReqDto)
    {
        try
        {
            var createdUser = await _userService.CreateAsync(userReqDto);
            createdUser.PasswordHashed = null;
            return Ok(createdUser);
        }
        catch (AuthenticationException exception)
        {
            return BadRequest(new Dictionary<string, string> {{"Message", exception.Message}});
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ActionName(nameof(Login))]
    public async Task<ActionResult> Login([FromBody] LoginUserReqDto user)
    {
        try
        {
            return Ok(await _userService.LoginAsync(user));
        }
        catch (AuthenticationException exception)
        {
            return BadRequest(new Dictionary<string, string> {{"Message", exception.Message}});
        }
    }

    [HttpGet("current")]
    [ActionName(nameof(CurrentUser))]
    public async Task<User> CurrentUser()
    {
        var userId = _authHelper.GetUserId(this);
        return await _userService.FindByIdAsync(userId);
    }
}