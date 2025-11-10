using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Infra.Data;

public class MongoContext
{
    private readonly IMongoDatabase _db;
    private readonly ILogger<MongoContext> _logger;

    public MongoContext(IConfiguration config, ILogger<MongoContext> logger)
    {
        _logger = logger;

        var connectionString = config.GetConnectionString("Mongo")
                               ?? config["Mongo:ConnectionString"];
        var databaseName = config["Mongo:DatabaseName"] ?? "payments_db";

        _logger.LogInformation("Conectando ao MongoDB. ConnectionString={ConnectionString}, Database={DatabaseName}", connectionString, databaseName);

        var client = new MongoClient(connectionString);
        _db = client.GetDatabase(databaseName);

        _logger.LogInformation("MongoContext inicializado com sucesso. Usando database '{DatabaseName}'", databaseName);
    }

    public IMongoCollection<Payment> Payments => _db.GetCollection<Payment>("payments");
}