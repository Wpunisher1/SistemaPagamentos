using BalanceApi.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace BalanceApi.Presentation.Endpoints;

public static class BalanceEndpoints
{
    public static void MapBalanceEndpoints(this IEndpointRouteBuilder app)
    {
        //  GET /balance/{accountId}
        app.MapGet("/api/v1/balance/{accountId}",
            async (BalanceHandler handler, string accountId, ILogger<Program> logger, CancellationToken ct) =>
            {
                logger.LogInformation("Requisição recebida para GET /balance/{AccountId}", accountId);

                var balance = await handler.GetByAccountAsync(accountId, ct);

                if (balance is null)
                {
                    logger.LogWarning("Nenhum saldo encontrado para: {AccountId}", accountId);
                    return Results.NotFound(new { error = "Conta não encontrada." });
                }

                logger.LogInformation("Retornando saldo para {AccountId}: Saldo={Saldo}", accountId, balance.Available);
                return Results.Ok(new
                {
                    accountId = balance.AccountId,
                    available = balance.Available
                });
            });

        // POST /balance/update
        app.MapPost("/api/v1/balance/update",
            async (BalanceHandler handler, [FromBody] BalanceUpdateRequest req, ILogger<Program> logger, CancellationToken ct) =>
            {
                try
                {
                    if (req == null)
                        return Results.BadRequest(new { error = "Requisição inválida." });

                    if (string.IsNullOrWhiteSpace(req.AccountId))
                        return Results.BadRequest(new { error = "AccountId é obrigatório." });

                    if (string.IsNullOrWhiteSpace(req.Operation))
                        return Results.BadRequest(new { error = "Operation é obrigatória." });

                    if (req.Amount <= 0)
                        return Results.BadRequest(new { error = "Amount deve ser maior que zero." });

                    logger.LogInformation("Atualizando saldo. AccountId={AccountId}, Operation={Operation}, Amount={Amount}",
                        req.AccountId, req.Operation, req.Amount);

                    var updated = await handler.UpdateAsync(req, ct);

                    logger.LogInformation("Saldo atualizado com sucesso. AccountId={AccountId}, NovoSaldo={Saldo}",
                        req.AccountId, updated.Available);

                    return Results.Ok(new
                    {
                        status = "Saldo Atualizado",
                        balance = new
                        {
                            accountId = updated.AccountId,
                            available = updated.Available
                        }
                    });
                }
                catch (ArgumentException ex)
                {
                    logger.LogWarning("Erro de validação ao atualizar saldo. AccountId={AccountId}, Erro={Erro}", req?.AccountId, ex.Message);
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    logger.LogWarning("Conflito ao atualizar saldo. AccountId={AccountId}, Operation={Operation}, Amount={Amount}, Erro={Erro}",
                        req?.AccountId, req?.Operation, req?.Amount, ex.Message);
                    return Results.Conflict(new { error = ex.Message });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Erro inesperado ao atualizar saldo. AccountId={AccountId}, Operation={Operation}, Amount={Amount}",
                        req?.AccountId, req?.Operation, req?.Amount);
                    return Results.Problem($"Erro inesperado: {ex.Message}");
                }
            });

        // Health check
        app.MapGet("/health", () => Results.Ok("ok"));
    }
}