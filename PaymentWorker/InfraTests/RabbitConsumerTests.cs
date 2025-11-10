using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentApi.CrossCuting.DTOs;
using PaymentWorker.Config;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Domain.Repositories;
using PaymentWorker.Messaging;
using Xunit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Threading;

public class RabbitConsumerTest
{
    [Fact]
    public async Task ProcessPaymentAsync_ShouldCreatePendingPayment_WhenOperationIsProcessing()
    {
        // Arrange
        var msg = new PaymentMessage
        {
            PaymentId = "pay001",
            AccountId = "acc123",
            Amount = 100,
            Operation = "processing"
        };

        var messageJson = JsonSerializer.Serialize(msg);

        var settings = Options.Create(new RabbitSettings
        {
            HostName = "localhost",
            QueueName = "test-queue",
            UserName = "guest",
            Password = "guest"
        });

        var balanceMock = new Mock<IBalanceClient>();
        var repoMock = new Mock<IPaymentRepository>();
        var loggerMock = new Mock<ILogger<RabbitConsumer>>();

        var consumer = new RabbitConsumer(settings, balanceMock.Object, repoMock.Object, loggerMock.Object);

        // Act
        var method = typeof(RabbitConsumer).GetMethod("ProcessPaymentAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        await (Task)method.Invoke(consumer, new object[] { messageJson, CancellationToken.None });

        // Assert
        repoMock.Verify(r => r.CreateAsync(It.Is<Payment>(p =>
            p.Id == msg.PaymentId &&
            p.AccountId == msg.AccountId &&
            p.Amount == msg.Amount &&
            p.Status == PaymentStatus.Pending &&
            p.Operation == "processing"
        ), It.IsAny<CancellationToken>()), Times.Once);
    }
}