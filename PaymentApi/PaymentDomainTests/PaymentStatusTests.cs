using Xunit;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Tests;

public class PaymentStatusTests
{
    [Fact]
    public void PaymentStatus_ShouldContainExpectedValues()
    {
        Assert.Equal(0, (int)PaymentStatus.Pending);
        Assert.Equal(1, (int)PaymentStatus.Processing);
        Assert.Equal(2, (int)PaymentStatus.Confirmed);
        Assert.Equal(3, (int)PaymentStatus.Rejected);
        Assert.Equal(4, (int)PaymentStatus.Cancelled);
        Assert.Equal(5, (int)PaymentStatus.Error);
    }
}