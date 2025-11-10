using BalanceApi.Domain.Handlers;
using Microsoft.Extensions.Logging;
using PaymentApi.CrossCuting.DTOs;
using PaymentWorker.Domain.Entities;
using PaymentWorker.Domain.Repositories;

namespace PaymentWorker.Domain.Handlers;

public class PaymentProcessHandler
{
    private readonly IPaymentRepository _repo;
    private readonly IBalanceClient _balanceClient;
    private readonly ILogger<PaymentProcessHandler> _logger;

    public PaymentProcessHandler(IPaymentRepository repo, IBalanceClient balanceClient, ILogger<PaymentProcessHandler> logger)
    {
        _repo = repo;
        _balanceClient = balanceClient;
        _logger = logger;
    }

    public async Task HandleAsync(PaymentMessage msg, CancellationToken ct)
    {
        _logger.LogInformation("[Worker] Processando pagamento {PaymentId} para conta {AccountId}, valor {Amount}",
            msg.PaymentId, msg.AccountId, msg.Amount);

        await _repo.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Processing, ct);

        try
        {
            var ok = await _balanceClient.UpdateAsync(new BalanceUpdateRequest
            {
                AccountId = msg.AccountId,
                Amount = msg.Amount,
                Operation = "debit"
            }, ct);

            if (ok)
            {
                await _repo.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Confirmed, ct);
                _logger.LogInformation("[Worker] Pagamento {PaymentId} confirmado com sucesso.", msg.PaymentId);
            }
            else
            {
                await _repo.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Rejected, ct);
                _logger.LogWarning("[Worker] Pagamento {PaymentId} rejeitado: saldo insuficiente.", msg.PaymentId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Worker] Erro inesperado ao processar pagamento {PaymentId}", msg.PaymentId);
            await _repo.UpdateStatusAsync(msg.PaymentId, PaymentStatus.Error, ct);
        }
    }
}
