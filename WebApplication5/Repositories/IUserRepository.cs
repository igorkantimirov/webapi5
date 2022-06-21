using WebApplication5.Models;

namespace WebApplication5.Repositories;

public interface IUserRepository
{
    Task<User> FindSingleAsync(string email, string username);
    Task<User> FindSingleByEmailAsync(string email);
    Task<User> FindSingleByIdAsync(string id);
    Task InsertOneAsync(User user);
}