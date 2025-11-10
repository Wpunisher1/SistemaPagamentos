using PaymentWorker.Config;
using Xunit;

public class MongoSettingsTest
{
    [Fact]
    public void ShouldAssignMongoSettingsCorrectly()
    {
        // Arrange
        var settings = new MongoSettings
        {
            ConnectionString = "mongodb://localhost:27017",
            Database = "test-db"
        };

        // Assert
        Assert.Equal("mongodb://localhost:27017", settings.ConnectionString);
        Assert.Equal("test-db", settings.Database);
    }
}