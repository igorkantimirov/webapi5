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
    [HttpPost("authenticate")]
    [ActionName(nameof(AuthenticateAsync))]
    public async Task<ActionResult> AuthenticateAsync([FromBody] AuthenticateAsyncReqDto dto)
    {
        try
        {
            return Ok(await _userService.AuthenticateAsync(dto));
        }
        catch (AuthenticationException exception)
        {
            return BadRequest(new Dictionary<string, string> {{"Message", exception.Message}});
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ActionName(nameof(RegisterAsync))]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterAsyncReqDto dto)
    {
        return Ok(await _userService.RegisterAsync(dto));
    }
    
    [HttpGet("")]
    [ActionName(nameof(GetAuthenticatedUserAsync))]
    public async Task<User> GetAuthenticatedUserAsync()
    {
        var userId = _authHelper.GetUserId(this);
        return await _userService.GetUserByIdAsync(userId);
    }
}