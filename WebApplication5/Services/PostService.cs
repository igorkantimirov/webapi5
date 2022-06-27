using WebApplication5.Models;
using WebApplication5.Repositories;
using WebApplication5.Services.DataTransferObjects;

namespace WebApplication5.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    
    public async Task CreateAsync(string userId, PostReqDto post)
    {
        await _postRepository.CreatePostAsync(userId, post.Title, post.Summary);
    }

    public Task<List<Post>> GetAllByUserId(string userId)
    {
        return _postRepository.GetAllByUserId(userId);
    }
}