using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using WebApplication5.Controllers;
using WebApplication5.Data;
using WebApplication5.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication5.Models;

namespace WebApplication5.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHelper _passwordHelper;
    
    public UserService(AppDbContext dbContext, IPasswordHelper passwordHelper)
    {
        _dbContext = dbContext;
        _passwordHelper = passwordHelper;
    }

    public async Task<AuthenticateAsyncResDto> AuthenticateAsync(AuthenticateAsyncReqDto dto)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == dto.Username);

        if (user == null)
            throw new AuthenticationException("User not found");

        if (_passwordHelper.GetHashedPassword(dto.Password) != user.PasswordHashed)
            throw new AuthenticationException("Password is not correct");
        
        return new AuthenticateAsyncResDto()
        {
            Email = user.Email,
            Id = user.Id,
            Username = user.Username,
            Token = CreateToken(user.Id.ToString())
        };
    }

    public async Task<RegisterAsyncResDto> RegisterAsync(RegisterAsyncReqDto dto)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Username == dto.Username);

        if (user != null)
            throw new InvalidOperationException("User is already registered");

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            LastLoginAt = DateTime.Now,
            PasswordHashed = _passwordHelper.GetHashedPassword(dto.Password)
        };
        
        await _dbContext.Users.AddAsync(newUser);
        await _dbContext.SaveChangesAsync();

        return new RegisterAsyncResDto();
    }

    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new AuthenticationException("User not found!");

        return user;
    }

    private string CreateToken(string userId)
    {
        var secret = Enumerable.Range(1, 129).ToList().ToString(); // todo: move to _appSettings
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, userId)}),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}