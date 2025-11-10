namespace PaymentApi.CrossCuting.DTOs;

public class PaymentMessage
{
    public string PaymentId { get; set; } = default!;
    public string AccountId { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Operation { get; set; } = default!; // "debit", "confirm", "cancel"
}