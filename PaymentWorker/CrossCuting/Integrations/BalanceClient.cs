using BalanceApi.Domain.Handlers;
using PaymentWorker.CrossCuting.DTOs;
using System.Net.Http.Json;

public class BalanceClient : IBalanceClient
{
    private readonly HttpClient _http;

    public BalanceClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<AccountBalanceDto?> GetAsync(string accountId, CancellationToken ct)
    {
        var response = await _http.GetAsync($"/api/v1/balance/{accountId}", ct);
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<AccountBalanceDto>(cancellationToken: ct);
    }

    public async Task<bool> UpdateAsync(BalanceUpdateRequest req, CancellationToken ct)
    {
        var response = await _http.PostAsJsonAsync("/api/v1/balance/update", req, ct);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(ct);
            Console.WriteLine($"[BalanceClient] Erro ao atualizar saldo: {error}");
            return false;
        }

        return true;
    }
}