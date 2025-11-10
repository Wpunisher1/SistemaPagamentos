namespace PaymentApi.Requests;

public class ConfirmRequest
{
    public string PaymentId { get; set; } = default!;
}