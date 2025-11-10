using BalanceApi.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

public class MongoContextTest
{
    [Fact]
    public void MongoContext_ShouldInitializeBalancesCollection()
    {
        // Arrange
        var settings = new MongoSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "test-db"
        };

        var optionsMock = new Mock<IOptions<MongoSettings>>();
        optionsMock.Setup(o => o.Value).Returns(settings);

        var loggerMock = new Mock<ILogger<MongoContext>>();

        // Act
        var context = new MongoContext(optionsMock.Object, loggerMock.Object);

        // Assert
        Assert.NotNull(context.Balances);
        Assert.Equal("balances", context.Balances.CollectionNamespace.CollectionName);
        Assert.Equal("test-db", context.Balances.Database.DatabaseNamespace.DatabaseName);
    }
}