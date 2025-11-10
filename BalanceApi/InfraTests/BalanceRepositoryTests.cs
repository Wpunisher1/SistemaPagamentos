using System.Threading;
using System.Threading.Tasks;
using BalanceApi.Infra.Data;
using MongoDB.Driver;
using Xunit;

public class BalanceRepositoryTest
{
    [Fact]
    public async Task GetAsync_ShouldReturnNull_WhenAccountNotExists()
    {
        // Arrange
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("test-db");
        var repository = new BalanceRepository(database);

        // Act
        var result = await repository.GetAsync("nonexistent", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}