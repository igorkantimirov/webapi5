using MongoDB.Bson;
using MongoDB.Driver;
using WebApplication5.Models;

namespace WebApplication5.Repositories;

public class PostRepository : IPostRepository
{
    private IMongoCollection<Post> _postCollection;

    public PostRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("MyCompany"); // todo: move to appsettings
        
        _postCollection = database.GetCollection<Post>("Posts");
    }

    public async Task<List<Post>> GetAllByUserId(string userId)
    {
        var posts = _postCollection.AsQueryable()
            .Where(x => x.OwnerId == userId)
            .ToList();
        return posts;
    }

    public async Task CreatePostAsync(string userId, string title, string summary)
    {
        await _postCollection.InsertOneAsync(new Post
        {
            Title = title,
            OwnerId =  userId,
            Summary = summary
        });
    }
}