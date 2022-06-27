using WebApplication5.Models;
using WebApplication5.Services.DataTransferObjects;

namespace WebApplication5.Services;

public interface IPostService
{
    Task CreateAsync(string userId, PostReqDto post);
    Task<List<Post>> GetAllByUserId(string userId);
}