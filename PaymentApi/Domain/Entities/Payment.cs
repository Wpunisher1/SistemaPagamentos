using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentApi.Domain.Entities;

public class Payment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("AccountId")]
    public string AccountId { get; set; } = default!;

    // Campo principal: Amount
    [BsonElement("Amount")]
    public decimal Amount { get; set; }

    // Alias para compatibilidade: Available
    [BsonElement("Available")]
    public decimal Available
    {
        get => Amount;
        set => Amount = value;
    }

    [BsonElement("Status")]
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [BsonElement("CreatedAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public object UpdatedAt { get; set; }
}