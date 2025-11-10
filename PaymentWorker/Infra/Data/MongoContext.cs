using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentWorker.Config;
using PaymentWorker.Domain.Entities;

namespace PaymentWorker.Infra.Data;

public class MongoContext
{
    private readonly IMongoDatabase _db;

    public MongoContext(IOptions<MongoSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _db = client.GetDatabase(options.Value.Database);
    }

    public IMongoCollection<Payment> Payments => _db.GetCollection<Payment>("payments");
}