using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

public interface IUserService
{
    Task<User> CreateAsync(CreateUserReqDto userReq);
    Task<Dictionary<string, string>> LoginAsync(LoginUserReqDto userReq);
    Task<User> FindByIdAsync(string userId);
}