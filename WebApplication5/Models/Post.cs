using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace WebApplication5.Models;

public class Post
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    [BsonElement]
    public string Title { get; set; }
    [BsonElement]
    public string Summary { get; set; }
    [BsonElement]
    public DateTime Added { get; set; }
    public string OwnerId { get; set; }
}