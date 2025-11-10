namespace PaymentApi.Requests;

public class PaymentRequest
{
    public string AccountId { get; set; } = default!;
    public decimal Amount { get; set; }
}