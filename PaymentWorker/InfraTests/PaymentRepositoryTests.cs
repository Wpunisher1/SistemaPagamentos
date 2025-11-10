using MongoDB.Driver;
using Moq;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Infra.Data;

public class PaymentRepositoryTest
{
    [Fact]
    public async Task GetAsync_ShouldReturnInsertedPayment()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        var database = client.GetDatabase("test-db");
        var repo = new PaymentRepository(database);

        var payment = new Payment
        {
            Id = Guid.NewGuid().ToString(), // garante que não vai duplicar
            AccountId = "acc123",
            Amount = 99.99m,
            Operation = "debit",
            Status = PaymentStatus.Pending
        };

        await repo.CreateAsync(payment, CancellationToken.None);

        var result = await repo.GetAsync("pay001", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("acc123", result.AccountId);
        Assert.Equal(99.99m, result.Amount);
    }
}