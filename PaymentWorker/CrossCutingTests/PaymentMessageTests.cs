using PaymentApi.CrossCuting.DTOs;
using Xunit;

public class PaymentMessageTest
{
    [Fact]
    public void ShouldCreatePaymentMessageWithCorrectValues()
    {
        // Arrange
        var message = new PaymentMessage
        {
            PaymentId = "pay001",
            AccountId = "acc123",
            Amount = 250.00m,
            Operation = "debit"
        };

        // Assert
        Assert.Equal("pay001", message.PaymentId);
        Assert.Equal("acc123", message.AccountId);
        Assert.Equal(250.00m, message.Amount);
        Assert.Equal("debit", message.Operation);
    }
}