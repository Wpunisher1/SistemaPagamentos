using MongoDB.Driver;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Domain.Repositories;

namespace PaymentWorker.Infra.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<Payment> _collection;

        public PaymentRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Payment>("payments");
        }

        public async Task CreateAsync(Payment payment, CancellationToken ct)
        {
            await _collection.InsertOneAsync(payment, cancellationToken: ct);
        }

        public async Task<Payment?> GetAsync(string paymentId, CancellationToken ct)
        {
            return await _collection
                .Find(x => x.Id == paymentId)
                .FirstOrDefaultAsync(ct);
        }

        public async Task UpdateStatusAsync(string paymentId, PaymentStatus status, CancellationToken ct)
        {
            var update = Builders<Payment>.Update
                .Set(x => x.Status, status)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(
                x => x.Id == paymentId,
                update,
                cancellationToken: ct
            );
        }
    }
}