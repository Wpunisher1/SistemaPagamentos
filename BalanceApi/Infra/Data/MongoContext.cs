using BalanceApi.Config;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;


public class MongoContext
{
    public IMongoCollection<AccountBalance> Balances { get; }

    public MongoContext(IOptions<MongoSettings> settings, ILogger<MongoContext> logger)
    {
        logger.LogInformation("Conectando ao MongoDB: {ConnectionString}", settings.Value.ConnectionString);
        logger.LogInformation("Usando banco: {DatabaseName}", settings.Value.DatabaseName);

        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);

        BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
        BsonSerializer.RegisterSerializer(typeof(decimal?), new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));

        Balances = database.GetCollection<AccountBalance>("balances");
    }
}