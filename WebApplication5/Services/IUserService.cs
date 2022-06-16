using WebApplication5.Models;
using WebApplication5.Services;

namespace WebApplication5.Controllers;

public interface IUserService
{
    Task<AuthenticateAsyncResDto> AuthenticateAsync(AuthenticateAsyncReqDto dto);
    Task<RegisterAsyncResDto> RegisterAsync(RegisterAsyncReqDto dto);
    Task<User> GetUserByIdAsync(Guid userId);
}