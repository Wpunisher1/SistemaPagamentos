using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BalanceApi.Domain.Handlers;
using PaymentWorker.CrossCuting.DTOs;
using Xunit;

public class BalanceClientTest
{
    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenResponseIsSuccess()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK));
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("http://localhost")
        };

        var client = new BalanceClient(httpClient);

        var request = new BalanceUpdateRequest
        {
            AccountId = "acc123",
            Amount = 100,
            Operation = "debit"
        };

        // Act
        var result = await client.UpdateAsync(request, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public FakeHttpMessageHandler(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}