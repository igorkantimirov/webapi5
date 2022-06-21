namespace WebApplication5.Helpers;

public interface IPasswordHelper
{
    string GetHashedPassword(string password);
    bool Verify(string password, string existingPasswordFromDb);
}