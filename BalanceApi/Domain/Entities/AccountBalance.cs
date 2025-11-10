using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class AccountBalance
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("AccountId")]
    public string AccountId { get; set; } = default!;

    [BsonElement("Available")]
    public decimal Available { get; set; }

    [BsonElement("UpdatedAt")]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("Status")]
    public string Status { get; set; } = default!;

    [BsonElement("CreatedAt")]
    public DateTime? CreatedAt { get; set; }
}