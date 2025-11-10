using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Moq;
using PaymentWorker.Config;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Infra.Data;
using Xunit;

public class MongoContextTest
{
    [Fact]
    public void ShouldInitializePaymentsCollection()
    {
        // Arrange
        var settings = new MongoSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            Database = "test-db"
        };

        var optionsMock = new Mock<IOptions<MongoSettings>>();
        optionsMock.Setup(o => o.Value).Returns(settings);

        // Act
        var context = new MongoContext(optionsMock.Object);

        // Assert
        Assert.NotNull(context.Payments);
        Assert.Equal("payments", context.Payments.CollectionNamespace.CollectionName);
        Assert.Equal("test-db", context.Payments.Database.DatabaseNamespace.DatabaseName);
    }
}