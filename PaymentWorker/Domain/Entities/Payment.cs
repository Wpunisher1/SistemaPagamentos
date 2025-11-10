namespace PaymentWorker.Domain.Entities;

public class Payment
{
    public string Id { get; set; } = default!;
    public string AccountId { get; set; } = default!;
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string Operation { get; set; }
}