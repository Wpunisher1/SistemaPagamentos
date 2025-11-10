namespace PaymentWorker.Domain.Entities;

public enum PaymentStatus
{
    Pending = 0,
    Processing = 1,
    Confirmed = 2,
    Rejected = 3,
    Cancelled = 4,
    Error = 5
}