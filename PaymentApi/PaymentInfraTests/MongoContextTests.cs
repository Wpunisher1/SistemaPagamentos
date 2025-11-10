using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PaymentApi.Infra.Data;

namespace PaymentApi.Tests;

public class MongoContextTests
{
    [Fact]
    public void MongoContext_ShouldInitializeAndExposePaymentsCollection()
    {
        // Arrange: configuração real em memória
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:Mongo", "mongodb://localhost:27017" },
                { "Mongo:DatabaseName", "test_db" }
            })
            .Build();

        var logger = new LoggerFactory().CreateLogger<MongoContext>();

        // Act
        var context = new MongoContext(config, logger);

        // Assert
        Assert.NotNull(context.Payments);
    }
}