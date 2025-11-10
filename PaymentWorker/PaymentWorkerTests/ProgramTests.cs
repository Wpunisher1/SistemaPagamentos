using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentWorker.Config;
using PaymentWorker.Domain.Handlers;
using PaymentWorker.Domain.Repositories;
using PaymentWorker.Infra.Data;
using PaymentWorker.Messaging;
using Xunit;

public class ProgramConfigurationTest
{
    [Fact]
    public void ShouldRegisterAllDependencies()
    {
        // Arrange
        var services = new ServiceCollection();

        // Simula configuração mínima
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["BalanceApi:BaseAddress"] = "http://localhost",
                ["MongoSettings:ConnectionString"] = "mongodb://localhost:27017",
                ["MongoSettings:Database"] = "test-db",
                ["Rabbit:HostName"] = "localhost",
                ["Rabbit:UserName"] = "guest",
                ["Rabbit:Password"] = "guest",
                ["Rabbit:QueueName"] = "test-queue"
            })
            .Build();

        services.AddSingleton<IConfiguration>(config);

        // Registra os mesmos serviços do Program.cs
        services.AddHostedService<RabbitConsumer>();
        services.AddScoped<PaymentProcessHandler>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IBalanceClient, BalanceClient>();

        services.AddHttpClient<IBalanceClient, BalanceClient>(client =>
        {
            var baseAddress = config["BalanceApi:BaseAddress"];
            client.BaseAddress = new Uri(baseAddress!);
        });

        services.AddSingleton<IMongoClient>(sp =>
        {
            var conn = config["MongoSettings:ConnectionString"];
            return new MongoClient(conn);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var dbName = config["MongoSettings:Database"];
            return client.GetDatabase(dbName);
        });

        services.Configure<RabbitSettings>(config.GetSection("Rabbit"));

        var provider = services.BuildServiceProvider();

        // Act & Assert
        Assert.NotNull(provider.GetService<PaymentProcessHandler>());
        Assert.NotNull(provider.GetService<IPaymentRepository>());
        Assert.NotNull(provider.GetService<IBalanceClient>());
        Assert.NotNull(provider.GetService<IMongoDatabase>());
        Assert.NotNull(provider.GetService<IOptions<RabbitSettings>>());
    }
}