using PaymentApi.Domain.Entities;
using PaymentApi.Domain.Services;
using PaymentApi.CrossCuting.DTOs;
using PaymentApi.Domain.Repositories;

namespace PaymentApi.Domain.Handlers;

public class PaymentHandler
{
    private readonly IPaymentRepository _repo;
    private readonly IMessageBus _bus;

    public PaymentHandler(IPaymentRepository repo, IMessageBus bus)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _bus = bus ?? throw new ArgumentNullException(nameof(bus));
    }

    public async Task<Payment> CreateAsync(string accountId, decimal amount, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(accountId))
            throw new ArgumentException("AccountId não pode ser vazio.");

        if (amount <= 0)
            throw new ArgumentException("Amount deve ser maior que zero.");

        var payment = new Payment
        {
            AccountId = accountId,
            Amount = amount,
            Status = PaymentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.CreateAsync(payment, ct);

        // Publica como "processing" 
        _bus.Publish(new PaymentMessage
        {
            PaymentId = payment.Id,
            AccountId = payment.AccountId,
            Amount = payment.Amount,
            Operation = "processing"
        });

        return payment;
    }

    public async Task ConfirmAsync(string paymentId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
            throw new ArgumentException("PaymentId é obrigatório.");

        var payment = await _repo.GetAsync(paymentId, ct);
        if (payment is null)
            throw new InvalidOperationException($"Pagamento {paymentId} não encontrado.");

        await _repo.UpdateStatusAsync(paymentId, PaymentStatus.Confirmed, ct);

        _bus.Publish(new PaymentMessage
        {
            PaymentId = payment.Id,
            AccountId = payment.AccountId,
            Amount = payment.Amount,
            Operation = "debit"
        });
    }

    public async Task CancelAsync(string paymentId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
            throw new ArgumentException("PaymentId é obrigatório.");

        var payment = await _repo.GetAsync(paymentId, ct);
        if (payment is null)
            throw new InvalidOperationException($"Pagamento {paymentId} não encontrado.");

        await _repo.UpdateStatusAsync(paymentId, PaymentStatus.Cancelled, ct);

        _bus.Publish(new PaymentMessage
        {
            PaymentId = payment.Id,
            AccountId = payment.AccountId,
            Amount = payment.Amount,
            Operation = "cancel"
        });
    }
}