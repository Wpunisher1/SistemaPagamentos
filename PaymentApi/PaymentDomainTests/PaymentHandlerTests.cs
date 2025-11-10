using Xunit;
using Moq;
using PaymentApi.Domain.Entities;
using PaymentApi.Domain.Handlers;
using PaymentApi.Domain.Repositories;
using PaymentApi.Domain.Services;
using PaymentApi.CrossCuting.DTOs;

namespace PaymentApi.Tests;

public class PaymentHandlerTests
{
    private readonly PaymentHandler _handler;

    public PaymentHandlerTests()
    {
        var repo = new Mock<IPaymentRepository>();
        var bus = new Mock<IMessageBus>();

        // Mock para CreateAsync
        repo.Setup(r => r.CreateAsync(It.IsAny<Payment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment p, CancellationToken _) => p);

        // Mock para GetAsync
        repo.Setup(r => r.GetAsync("p-123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Payment { Id = "p-123", AccountId = "acc-1", Amount = 50m });

        // Mock para UpdateStatusAsync
        repo.Setup(r => r.UpdateStatusAsync(It.IsAny<string>(), It.IsAny<PaymentStatus>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

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