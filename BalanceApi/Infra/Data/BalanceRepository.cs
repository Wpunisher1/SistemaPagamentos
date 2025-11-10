using MongoDB.Driver;
using BalanceApi.Domain.Repositories;

namespace BalanceApi.Infra.Data
{
    public class BalanceRepository : IBalanceRepository
    {
        private readonly IMongoCollection<AccountBalance> _balances;

        public BalanceRepository(IMongoDatabase database)
        {
     
            _balances = database.GetCollection<AccountBalance>("balances");
        }

        public async Task<AccountBalance?> GetAsync(string accountId, CancellationToken ct)
        {
            var filter = Builders<AccountBalance>.Filter.Eq(b => b.AccountId, accountId);
            return await _balances.Find(filter).FirstOrDefaultAsync(ct);
        }

        public async Task UpsertAsync(AccountBalance balance, CancellationToken ct)
        {
            var filter = Builders<AccountBalance>.Filter.Eq(b => b.AccountId, balance.AccountId);
            await _balances.ReplaceOneAsync(filter, balance, new ReplaceOptions { IsUpsert = true }, ct);
        }
    }
}