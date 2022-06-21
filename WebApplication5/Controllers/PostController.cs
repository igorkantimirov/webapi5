using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Helpers;
using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services.DataTransferObjects;

namespace WebApplication5.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IAuthHelper _authHelper;

    public PostController(IPostRepository postRepository, IAuthHelper authHelper)
    {
        _postRepository = postRepository;
        _authHelper = authHelper;
    }


    [AllowAnonymous]
    [HttpPost("create")]
    [ActionName(nameof(Create))]
    public async Task<ActionResult> Create([FromBody] PostReqDto post)
    {
        var userId = _authHelper.GetUserId(this);
        await _postRepository.CreatePostAsync(userId, post.Title, post.Summary);
        return Ok("");
    }
    
    
    [AllowAnonymous]
    [HttpGet("")]
    [ActionName(nameof(GetAll))]
    public async Task<ActionResult> GetAll()
    {
        var userId = _authHelper.GetUserId(this);
        var posts = await _postRepository.GetAllByUserId(userId);
        return Ok(posts);
    }
}