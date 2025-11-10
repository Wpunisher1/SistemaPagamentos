using Xunit;
using MongoDB.Driver;
using PaymentApi.Domain.Entities;
using PaymentWorker.Infra.Data;

namespace PaymentApi.Tests;

public class PaymentRepositoryLiveTests
{
    private readonly IMongoDatabase _db;
    private readonly PaymentRepository _repo;

    public PaymentRepositoryLiveTests()
    {
        var client = new MongoClient("mongodb://localhost:27017");
        _db = client.GetDatabase("test_db");
        _repo = new PaymentRepository(_db);
    }

    [Fact]
    public async Task CreateAndGet_ShouldWork()
    {
        var payment = new Payment
        {
            AccountId = "acc-99",
            Amount = 999m
        };

        await _repo.CreateAsync(payment, CancellationToken.None);
        var result = await _repo.GetAsync(payment.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("acc-99", result!.AccountId);
    }
}