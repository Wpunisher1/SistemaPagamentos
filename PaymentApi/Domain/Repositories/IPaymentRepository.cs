using PaymentApi.Domain.Entities;

namespace PaymentApi.Domain.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment, CancellationToken ct);
        Task<Payment?> GetAsync(string id, CancellationToken ct);
        Task UpdateStatusAsync(string id, PaymentStatus status, CancellationToken ct);
    }
}