using BalanceApi.Domain.Handlers;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentApi.CrossCuting.DTOs;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Domain.Handlers;
using PaymentWorker.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;

public class PaymentProcessHandlerTest
{
    [Fact]
    public async Task HandleAsync_ShouldConfirmPayment_WhenBalanceIsOk()
    {
        // Arrange
        var paymentId = "pay123";
        var accountId = "acc456";
        var amount = 100m;

        var msg = new PaymentMessage
        {
            PaymentId = paymentId,
            AccountId = accountId,
            Amount = amount
        };

        var repoMock = new Mock<IPaymentRepository>();
        var balanceMock = new Mock<IBalanceClient>();
        var loggerMock = new Mock<ILogger<PaymentProcessHandler>>();

        repoMock.Setup(r => r.UpdateStatusAsync(paymentId, PaymentStatus.Processing, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        balanceMock.Setup(b => b.UpdateAsync(It.IsAny<BalanceUpdateRequest>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(true);

        repoMock.Setup(r => r.UpdateStatusAsync(paymentId, PaymentStatus.Confirmed, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        var handler = new PaymentProcessHandler(repoMock.Object, balanceMock.Object, loggerMock.Object);

        // Act
        await handler.HandleAsync(msg, CancellationToken.None);

        // Assert
        repoMock.Verify(r => r.UpdateStatusAsync(paymentId, PaymentStatus.Processing, It.IsAny<CancellationToken>()), Times.Once);
        repoMock.Verify(r => r.UpdateStatusAsync(paymentId, PaymentStatus.Confirmed, It.IsAny<CancellationToken>()), Times.Once);
        balanceMock.Verify(b => b.UpdateAsync(It.Is<BalanceUpdateRequest>(r =>
            r.AccountId == accountId &&
            r.Amount == amount &&
            r.Operation == "debit"
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}