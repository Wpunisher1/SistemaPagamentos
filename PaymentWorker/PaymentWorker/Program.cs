using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using PaymentWorker.Domain.Handlers;
using PaymentWorker.Domain.Repositories;
using PaymentWorker.Infra.Data;
using PaymentWorker.Messaging;
using PaymentWorker.Config; // importante para RabbitSettings

namespace PaymentWorker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    // Worker que consome mensagens do RabbitMQ
                    services.AddHostedService<RabbitConsumer>();

                    // Handlers e repositórios
                    services.AddScoped<PaymentProcessHandler>();
                    services.AddScoped<IPaymentRepository, PaymentRepository>();
                    services.AddScoped<IBalanceClient, BalanceClient>();

                    // HttpClient para BalanceApi
                    services.AddHttpClient<IBalanceClient, BalanceClient>(client =>
                    {
                        var baseAddress = configuration["BalanceApi:BaseAddress"];
                        client.BaseAddress = new Uri(baseAddress!);
                    });

                    // Configuração do MongoDB
                    services.AddSingleton<IMongoClient>(sp =>
                    {
                        var conn = configuration["MongoSettings:ConnectionString"];
                        return new MongoClient(conn);
                    });

                    services.AddScoped<IMongoDatabase>(sp =>
                    {
                        var client = sp.GetRequiredService<IMongoClient>();
                        var dbName = configuration["MongoSettings:Database"];
                        return client.GetDatabase(dbName);
                    });

                    // Configurações do Rabbit via Options
                    services.Configure<RabbitSettings>(configuration.GetSection("Rabbit"));
                })
                .Build();

            await host.RunAsync();
        }
    }
}