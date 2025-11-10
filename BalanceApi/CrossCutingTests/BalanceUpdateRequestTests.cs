using Xunit;

public class BalanceUpdateRequestTest
{
    [Fact]
    public void ShouldCreateRequestWithCorrectValues()
    {
        // Arrange
        var request = new BalanceUpdateRequest
        {
            AccountId = "acc123",
            Amount = 250.75m,
            Operation = "credit"
        };

        // Assert
        Assert.Equal("acc123", request.AccountId);
        Assert.Equal(250.75m, request.Amount);
        Assert.Equal("credit", request.Operation);
    }
}