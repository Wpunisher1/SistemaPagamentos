using BalanceApi.Domain.Repositories;

namespace BalanceApi.Domain.Handlers
{
    public class BalanceHandler
    {
        private readonly IBalanceRepository _repo;

        public BalanceHandler(IBalanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<AccountBalance?> GetByAccountAsync(string accountId, CancellationToken ct)
        {
            return await _repo.GetAsync(accountId, ct);
        }

        public async Task<AccountBalance> UpdateAsync(BalanceUpdateRequest req, CancellationToken ct)
        {
            var existing = await _repo.GetAsync(req.AccountId, ct);
            var saldoAtual = existing?.Available ?? 0;

            if (req.Operation.ToLower() == "debit")
            {
                if (saldoAtual < req.Amount)
                    throw new InvalidOperationException("Saldo insuficiente.");
                saldoAtual -= req.Amount;
            }
            else if (req.Operation.ToLower() == "credit")
            {
                saldoAtual += req.Amount;
            }

            var updated = new AccountBalance
            {
                Id = existing?.Id ?? MongoDB.Bson.ObjectId.GenerateNewId(),
                AccountId = req.AccountId,
                Available = saldoAtual,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = existing?.CreatedAt ?? DateTime.UtcNow,
                Status = "Saldo Atualizado" 
            };

            await _repo.UpsertAsync(updated, ct);
            return updated;
        }
    }
}