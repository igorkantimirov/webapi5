using System.Security.Cryptography;
using System.Text;

namespace WebApplication5.Helpers;

public class PasswordHelper : IPasswordHelper
{
    public string GetHashedPassword(string password)
    {
        if (password == null) 
            throw new ArgumentNullException(nameof(password));

        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

        using var hmac = new HMACSHA512();
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(password)).ToString() ?? throw new InvalidOperationException();
    }
}