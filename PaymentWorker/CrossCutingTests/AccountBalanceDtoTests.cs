using System;
using PaymentWorker.CrossCuting.DTOs;
using Xunit;

public class AccountBalanceDtoTest
{
    [Fact]
    public void ShouldCalculateTotalCorrectly()
    {
        // Arrange
        var dto = new AccountBalanceDto
        {
            AccountId = "acc123",
            Available = 150.75m,
            Blocked = 49.25m,
            UpdatedAt = DateTime.UtcNow
        };

        // Assert
        Assert.Equal("acc123", dto.AccountId);
        Assert.Equal(150.75m, dto.Available);
        Assert.Equal(49.25m, dto.Blocked);
        Assert.Equal(200.00m, dto.Total);
    }
}