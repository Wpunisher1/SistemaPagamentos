namespace PaymentWorker.Config
{
    public class RabbitSettings
    {
        public string HostName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string QueueName { get; set; } = null!;
    }
}