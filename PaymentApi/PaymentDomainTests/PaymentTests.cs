using Xunit;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Tests;

public class PaymentTests
{
    [Fact]
    public void Payment_ShouldInitializeWithDefaults()
    {
        // Arrange
        var payment = new Payment
        {
            AccountId = "",
            Amount = 0m,
            UpdatedAt = null!
        };

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(payment.Id));
        Assert.Equal(0m, payment.Amount);
        Assert.Equal(0m, payment.Available); // alias
        Assert.Equal(PaymentStatus.Pending, payment.Status);
        Assert.NotEqual(default, payment.CreatedAt);
    }
}