using MongoDB.Driver;
using WebApplication5.Models;

namespace WebApplication5.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _usersCollection;
    
    public UserRepository(IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase("MyCompany");
        _usersCollection = mongoDatabase.GetCollection<User>("Users");
    }
    
    public async Task<User> FindSingleAsync(string email, string username)
    {
        var existingUsers = await _usersCollection.FindAsync(x => x.Username == username || x.Email == email);
        return await existingUsers.FirstOrDefaultAsync();
    }

    public async Task<User> FindSingleByEmailAsync(string email)
    {
        return await (await _usersCollection.FindAsync(x => x.Email == email))
            .FirstOrDefaultAsync();
    }

    public async Task<User> FindSingleByIdAsync(string id)
    {
        return await (await _usersCollection.FindAsync(x => x._id == id))
            .FirstOrDefaultAsync();
    }

    public async Task InsertOneAsync(User user)
    {
        await _usersCollection.InsertOneAsync(user);
    }
}