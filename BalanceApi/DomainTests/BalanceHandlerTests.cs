using System;
using System.Threading;
using System.Threading.Tasks;
using BalanceApi.Domain.Handlers;
using BalanceApi.Domain.Repositories;
using Moq;
using Xunit;

public class BalanceHandlerTest
{
    [Fact]
    public async Task UpdateAsync_WithCredit_ShouldIncreaseBalance()
    {
        // Arrange
        var repoMock = new Mock<IBalanceRepository>();
        var accountId = "acc123";
        var existingBalance = new AccountBalance
        {
            Id = MongoDB.Bson.ObjectId.GenerateNewId(),
            AccountId = accountId,
            Available = 100,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddDays(-1),
            Status = "Inicial"
        };

        repoMock.Setup(r => r.GetAsync(accountId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingBalance);

        repoMock.Setup(r => r.UpsertAsync(It.IsAny<AccountBalance>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        var handler = new BalanceHandler(repoMock.Object);

        var request = new BalanceUpdateRequest
        {
            AccountId = accountId,
            Operation = "credit",
            Amount = 50
        };

        // Act
        var result = await handler.UpdateAsync(request, CancellationToken.None);

        // Assert
        Assert.Equal(150, result.Available);
        Assert.Equal("Saldo Atualizado", result.Status);
        Assert.Equal(accountId, result.AccountId);
    }
}