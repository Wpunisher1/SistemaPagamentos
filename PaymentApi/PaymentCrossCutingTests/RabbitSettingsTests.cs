using PaymentApi.CrossCuting.Config;
using Xunit;

namespace PaymentApi.Tests;

public class RabbitSettingsTests
{
    [Fact]
    public void RabbitSettings_ShouldHaveDefaultValues()
    {
        var settings = new RabbitSettings();

        Assert.Equal("rabbitmq", settings.HostName);
        Assert.Equal("admin", settings.UserName);
        Assert.Equal("admin", settings.Password);
        Assert.Equal("payment-process-topic", settings.QueueName);
    }
}