namespace PaymentWorker.CrossCuting.DTOs;

public class AccountBalanceDto
{
    public string AccountId { get; set; } = string.Empty;
    public decimal Available { get; set; }
    public decimal Blocked { get; set; }
    public DateTime UpdatedAt { get; set; }

    public decimal Total => Available + Blocked;
}