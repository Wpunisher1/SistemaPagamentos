public class BalanceUpdateRequest
{
    public string AccountId { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Operation { get; set; } = default!;
}