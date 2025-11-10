using System;
using PaymentWorker.Domain.Entities;
using Xunit;

public class PaymentTest
{
    [Fact]
    public void ShouldCreatePaymentWithCorrectValues()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var payment = new Payment
        {
            Id = "pay001",
            AccountId = "acc123",
            Amount = 150.50m,
            Operation = "debit",
            Status = PaymentStatus.Confirmed,
            CreatedAt = now,
            UpdatedAt = now
        };

        // Assert
        Assert.Equal("pay001", payment.Id);
        Assert.Equal("acc123", payment.AccountId);
        Assert.Equal(150.50m, payment.Amount);
        Assert.Equal("debit", payment.Operation);
        Assert.Equal(PaymentStatus.Confirmed, payment.Status);
        Assert.Equal(now, payment.CreatedAt);
        Assert.Equal(now, payment.UpdatedAt);
    }
}