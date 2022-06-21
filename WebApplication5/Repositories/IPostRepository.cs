using WebApplication5.Models;

namespace WebApplication5.Repositories;

public interface IPostRepository
{
    Task<List<Post>> GetAllByUserId(string userId);
    Task CreatePostAsync(string userId, string title, string summary);
}