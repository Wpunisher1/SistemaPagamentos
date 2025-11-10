using Xunit;
using Microsoft.Extensions.Options;
using PaymentApi.CrossCuting.Config;
using PaymentApi.CrossCuting.DTOs;
using PaymentApi.Infra.Messaging;

namespace PaymentApi.Tests;

public class RabbitMessageBusTests
{
    [Fact]
    public void Publish_ShouldSendMessage_WhenRabbitIsAvailable()
    {
        // Arrange: configurações reais para conexão local
        var settings = new RabbitSettings
        {
            HostName = "localhost",
            UserName = "admin",
            Password = "admin",
            QueueName = "test-queue"
        };

        var options = Options.Create(settings);

        using var bus = new RabbitMessageBus(options);

        var message = new PaymentMessage
        {
            PaymentId = "p-123",
            AccountId = "acc-1",
            Amount = 50m,
            Operation = "test"
        };

        // Act & Assert: garante que não lança exceção
        bus.Publish(message);
        Assert.True(true);
    }
}