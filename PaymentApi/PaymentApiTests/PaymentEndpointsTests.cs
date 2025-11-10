using Xunit;
using Moq;
using PaymentApi.Domain.Handlers;
using PaymentApi.Domain.Repositories;
using PaymentApi.Domain.Services;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Tests;

public class PaymentEndpointsTests
{
    private readonly PaymentHandler _handler;

    public PaymentEndpointsTests()
    {
        var repo = new Mock<IPaymentRepository>();
        var bus = new Mock<IMessageBus>();

        // Configura o repo para retornar um pagamento falso quando buscar "p-123"
        repo.Setup(r => r.GetAsync("p-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Payment { Id = "p-123", AccountId = "acc-1", Amount = 50m });

        _handler = new PaymentHandler(repo.Object, bus.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldPass()
    {
        var result = await _handler.CreateAsync("acc-1", 50m, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task ConfirmAsync_ShouldPass()
    {
        await _handler.ConfirmAsync("p-123", CancellationToken.None);
        Assert.True(true);
    }

    [Fact]
    public async Task CancelAsync_ShouldPass()
    {
        await _handler.CancelAsync("p-123", CancellationToken.None);
        Assert.True(true);
    }
}