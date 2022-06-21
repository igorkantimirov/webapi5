using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication5.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _id { get; set; }
    [BsonElement]
    public string Username { get; set; }
    [BsonElement]
    public string Email { get; set; }
    [BsonElement]
    public DateTime CreatedAt { get; set; }
    [BsonElement]
    public DateTime? UpdatedAt { get; set; }
    [BsonElement]
    public DateTime? LastLoginAt { get; set; }
    [BsonElement]
    public string PasswordHashed { get; set; }
}