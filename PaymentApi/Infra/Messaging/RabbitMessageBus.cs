using Microsoft.Extensions.Options;
using PaymentApi.CrossCuting.Config;
using PaymentApi.CrossCuting.DTOs;
using PaymentApi.Domain.Services;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PaymentApi.Infra.Messaging
{
    public class RabbitMessageBus : IMessageBus, IDisposable
    {
        private readonly RabbitSettings _settings;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMessageBus(IOptions<RabbitSettings> options)
        {
            _settings = options.Value;

            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = factory.CreateConnection("paymentapi-client");
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public void Publish(PaymentMessage message)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(
                exchange: "",
                routingKey: _settings.QueueName,
                basicProperties: null,
                body: body
            );

            Console.WriteLine($"[Rabbit] Mensagem publicada: {JsonSerializer.Serialize(message)}");
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}