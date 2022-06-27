using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Helpers;
using WebApplication5.Repositories;
using WebApplication5.Services;
using WebApplication5.Services.DataTransferObjects;

namespace WebApplication5.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IAuthHelper _authHelper;

    public PostController(IPostService postService, IAuthHelper authHelper)
    {
        _postService = postService;
        _authHelper = authHelper;
    }

    [AllowAnonymous]
    [HttpPost("create")]
    [ActionName(nameof(Create))]
    public async Task<ActionResult> Create([FromBody] PostReqDto post)
    {
        var userId = _authHelper.GetUserId(this);
        await _postService.CreateAsync(userId, post);
        return Ok("");
    }
    
    
    [AllowAnonymous]
    [HttpGet("")]
    [ActionName(nameof(GetAll))]
    public async Task<ActionResult> GetAll()
    {
        var userId = _authHelper.GetUserId(this);
        var posts = await _postService.GetAllByUserId(userId);
        return Ok(posts);
    }
}