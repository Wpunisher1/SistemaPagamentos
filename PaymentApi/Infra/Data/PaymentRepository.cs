using MongoDB.Driver;
using PaymentApi.Domain.Entities;
using PaymentApi.Domain.Repositories;

namespace PaymentWorker.Infra.Data
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentRepository(IMongoDatabase database)
        {
            _payments = database.GetCollection<Payment>("payments");
        }

        public async Task<Payment> CreateAsync(Payment payment, CancellationToken ct)
        {
            await _payments.InsertOneAsync(payment, cancellationToken: ct);
            return payment;
        }

        public async Task<Payment?> GetAsync(string id, CancellationToken ct)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.Id, id);
            return await _payments.Find(filter).FirstOrDefaultAsync(ct);
        }

        public async Task UpdateStatusAsync(string id, PaymentStatus status, CancellationToken ct)
        {
            var filter = Builders<Payment>.Filter.Eq(p => p.Id, id);
            var update = Builders<Payment>.Update
                .Set(p => p.Status, status)
                .Set(p => p.UpdatedAt, DateTime.UtcNow); // Atualiza também o timestamp

            await _payments.UpdateOneAsync(filter, update, cancellationToken: ct);
        }
    }
}