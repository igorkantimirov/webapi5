using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using WebApplication5.Controllers;
using WebApplication5.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public class UserService : IUserService
{
    private readonly IPasswordHelper _passwordHelper;
    private readonly IUserRepository _userRepository;
    
    public UserService(IPasswordHelper passwordHelper, IUserRepository userRepository)
    {
        _passwordHelper = passwordHelper;
        _userRepository = userRepository;
    }

    public async Task<User> CreateAsync(CreateUserReqDto userReq)
    {
        var existingUser = await _userRepository.FindSingleAsync(userReq.Email, userReq.Username);
        
        if (existingUser != null)
        {
            throw new AuthenticationException("User already exists");
        }
        
        var newUser = new User
        {
            Email = userReq.Email,
            Username = userReq.Username,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            PasswordHashed = _passwordHelper.GetHashedPassword(userReq.Password)
        };

        await _userRepository.InsertOneAsync(newUser);
        
        return newUser;
    }

    public async Task<string> LoginAsync(LoginUserReqDto userReq)
    {
        var existingUser = await _userRepository.FindSingleByEmailAsync(userReq.Email);
        
        if (existingUser == null)
        {
            throw new AuthenticationException("User doesn't exist");
        }

        if (_passwordHelper.Verify(userReq.Password, existingUser.PasswordHashed))
        {
            return CreateToken(existingUser._id);
        }

        throw new AuthenticationException("Password is not correct");
    }

    public async Task<User> FindByIdAsync(string userId)
    {
        return await _userRepository.FindSingleByIdAsync(userId);
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