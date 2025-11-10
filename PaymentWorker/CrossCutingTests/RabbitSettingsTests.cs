using PaymentWorker.Config;
using Xunit;

public class RabbitSettingsTest
{
    [Fact]
    public void ShouldAssignRabbitSettingsCorrectly()
    {
        // Arrange
        var settings = new RabbitSettings
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
            QueueName = "payments-queue"
        };

        // Assert
        Assert.Equal("localhost", settings.HostName);
        Assert.Equal("guest", settings.UserName);
        Assert.Equal("guest", settings.Password);
        Assert.Equal("payments-queue", settings.QueueName);
    }
}