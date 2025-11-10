namespace PaymentApi.CrossCuting.Config
{
    public class RabbitSettings
    {
        public string HostName { get; set; } = "rabbitmq";
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin";
        public string QueueName { get; set; } = "payment-process-topic";
    }
}