using PaymentApi.Requests;

public class CancelRequestTests
{
    [Fact]
    public void CancelRequest_Should_Set_And_Get_PaymentId()
    {
        var request = new CancelRequest();
        request.PaymentId = "abc123";
        Assert.Equal("abc123", request.PaymentId);
    }

    [Fact]
    public void CancelRequest_ShouldHavePaymentIdProperty()
    {
        var request = new CancelRequest { PaymentId = "abc" };
        Assert.Equal("abc", request.PaymentId);
    }
}