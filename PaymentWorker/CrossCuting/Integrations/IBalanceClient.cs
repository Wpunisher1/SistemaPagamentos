using BalanceApi.Domain.Handlers;
using PaymentWorker.CrossCuting.DTOs;

public interface IBalanceClient
{
    Task<AccountBalanceDto?> GetAsync(string accountId, CancellationToken ct);
    Task<bool> UpdateAsync(BalanceUpdateRequest req, CancellationToken ct);
}