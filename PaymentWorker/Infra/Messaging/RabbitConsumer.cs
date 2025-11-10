using BalanceApi.Domain.Handlers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentApi.CrossCuting.DTOs;
using PaymentWorker.Config;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Domain.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PaymentWorker.Messaging
{
    public class RabbitConsumer : BackgroundService
    {
        private readonly RabbitSettings _settings;
        private readonly IBalanceClient _balanceClient;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<RabbitConsumer> _logger;
        private IConnection? _connection;
        private IModel? _channel;

        public RabbitConsumer(
            IOptions<RabbitSettings> options,
            IBalanceClient balanceClient,
            IPaymentRepository paymentRepository,
            ILogger<RabbitConsumer> logger)
        {
            _settings = options.Value;
            _balanceClient = balanceClient;
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = factory.CreateConnection("paymentworker-client");
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("[Worker] Mensagem recebida: {Message}", message);

                try
                {
                    await ProcessPaymentAsync(message, stoppingToken);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "[Worker] Erro ao processar mensagem");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            _channel.BasicConsume(
                queue: _settings.QueueName,
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        private async Task ProcessPaymentAsync(string message, CancellationToken ct)
        {
            _logger.LogInformation("[Worker] Processando pagamento: {Message}", message);

            PaymentMessage? msg;
            try
            {
                msg = JsonSerializer.Deserialize<PaymentMessage>(message);
                if (msg is null)
                {
                    _logger.LogWarning("[Worker] Erro: mensagem inválida.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Worker] Erro ao desserializar mensagem");
                return;
            }

            switch (msg.Operation.ToLowerInvariant())
            {
                case "processing":
                    _logger.LogInformation("[Worker] Criando pagamento como Pending...");

                    var pendingPayment = new Payment
                    {
                        Id = msg.PaymentId,
                        AccountId = msg.AccountId,
                        Amount = msg.Amount,
                        Operation = msg.Operation,
                        Status = PaymentStatus.Pending,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _paymentRepository.CreateAsync(pendingPayment, ct);
                    _logger.LogInformation("[Worker] Pagamento criado com status Pending.");
                    break;

                case "debit":
                    _logger.LogInformation("[Worker] Tentando debitar pagamento {PaymentId}...", msg.PaymentId);

                    var updateRequest = new BalanceUpdateRequest
                    {
                        AccountId = msg.AccountId,
                        Amount = msg.Amount,
                        Operation = "debit"
                    };

                    var success = await _balanceClient.UpdateAsync(updateRequest, ct);

                    if (success)
                    {
                        await _paymentRepository.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Confirmed, ct);
                        _logger.LogInformation("[Worker] Saldo debitado com sucesso da conta {AccountId}. Pagamento confirmado.", msg.AccountId);
                    }
                    else
                    {
                        await _paymentRepository.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Rejected, ct);
                        _logger.LogWarning("[Worker] Saldo insuficiente para conta {AccountId}. Pagamento rejeitado.", msg.AccountId);
                    }
                    break;

                case "cancel":
                    _logger.LogInformation("[Worker] Cancelando pagamento {PaymentId}...", msg.PaymentId);
                    await _paymentRepository.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Cancelled, ct);
                    break;

                default:
                    _logger.LogWarning("[Worker] Operação desconhecida: {Operation}", msg.Operation);
                    break;
            }
        }
    }
}