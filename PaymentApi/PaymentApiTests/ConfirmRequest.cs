using PaymentApi.Requests;
using Xunit;

public class ConfirmRequestTests
{
    [Fact]
    public void ConfirmRequest_Should_Set_And_Get_PaymentId()
    {
        // Arrange
        var request = new ConfirmRequest();

        // Act
        request.PaymentId = "abc123";

        // Assert
        Assert.Equal("abc123", request.PaymentId);
    }

    [Fact]
    public void ConfirmRequest_Default_Should_Be_Null()
    {
        // Arrange
        var request = new ConfirmRequest();

        // Assert
        Assert.Null(request.PaymentId);
    }
}