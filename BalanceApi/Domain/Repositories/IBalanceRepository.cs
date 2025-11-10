

namespace BalanceApi.Domain.Repositories;

public interface IBalanceRepository
{
    Task<AccountBalance?> GetAsync(string accountId, CancellationToken ct);
    Task UpsertAsync(AccountBalance balance, CancellationToken ct);
}