using PaymentApi.Domain.Handlers;
using PaymentApi.Requests;

namespace PaymentApi.Presentation.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        // Criação de pagamento (sempre Pending)
        app.MapPost("/api/v1/payment", async (PaymentHandler handler, PaymentRequest req, CancellationToken ct) =>
        {
            try
            {
                var payment = await handler.CreateAsync(req.AccountId, req.Amount, ct);
                return Results.Created(
                    $"/api/v1/payment/{payment.Id}",
                    new { PaymentId = payment.Id, Status = payment.Status.ToString() }
                );
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro inesperado: {ex.Message}");
            }
        });

        // Confirmação de pagamento
        app.MapPost("/api/v1/payment/confirm", async (PaymentHandler handler, ConfirmRequest req, CancellationToken ct) =>
        {
            try
            {
                await handler.ConfirmAsync(req.PaymentId, ct);
                return Results.Accepted(
                    $"/api/v1/payment/{req.PaymentId}",
                    new { PaymentId = req.PaymentId, Status = "Confirmed" }
                );
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro inesperado: {ex.Message}");
            }
        });

        // Cancelamento de pagamento
        app.MapPost("/api/v1/payment/cancel", async (PaymentHandler handler, CancelRequest req, CancellationToken ct) =>
        {
            try
            {
                await handler.CancelAsync(req.PaymentId, ct);
                return Results.Accepted(
                    $"/api/v1/payment/{req.PaymentId}",
                    new { PaymentId = req.PaymentId, Status = "Cancelled" }
                );
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro inesperado: {ex.Message}");
            }
        });

        // Health check
        app.MapGet("/health", () => Results.Ok("ok"));
    }
}