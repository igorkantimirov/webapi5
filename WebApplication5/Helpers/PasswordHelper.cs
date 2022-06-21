using System.Security.Cryptography;
using System.Text;
using BCrypt = BCrypt.Net.BCrypt;

namespace WebApplication5.Helpers;

public class PasswordHelper : IPasswordHelper
{
    public string GetHashedPassword(string password)
    {
        if (password == null) 
            throw new ArgumentNullException(nameof(password));

        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
        
        return global::BCrypt.Net.BCrypt.HashPassword(password) ?? throw new InvalidOperationException();
    }

    public bool Verify(string password, string existingPasswordFromDb)
    {
        return global::BCrypt.Net.BCrypt.Verify(password, existingPasswordFromDb);
    }
}