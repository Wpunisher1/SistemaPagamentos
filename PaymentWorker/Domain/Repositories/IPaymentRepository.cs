using PaymentWorker.Domain.Entities;

namespace PaymentWorker.Domain.Repositories;

public interface IPaymentRepository
{
    Task CreateAsync(Payment payment, CancellationToken ct);
    Task<Payment?> GetAsync(string paymentId, CancellationToken ct);
    Task UpdateStatusAsync(string paymentId, PaymentStatus status, CancellationToken ct);
}