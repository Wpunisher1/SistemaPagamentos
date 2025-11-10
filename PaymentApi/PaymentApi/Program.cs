using MongoDB.Driver;
using PaymentApi.CrossCuting.Config;
using PaymentApi.Domain.Handlers;
using PaymentApi.Domain.Repositories;
using PaymentApi.Domain.Services;
using PaymentApi.Infra.Data;
using PaymentApi.Infra.Messaging;
using PaymentApi.Presentation.Endpoints;
using PaymentWorker.Infra.Data;

var builder = WebApplication.CreateBuilder(args);

// Só registra serviços pesados se não for ambiente de teste
if (!builder.Environment.IsEnvironment("Testing"))
{
    // Configurações RabbitMQ
    builder.Services.Configure<RabbitSettings>(
        builder.Configuration.GetSection("Rabbit"));

    // MongoDB
    builder.Services.AddSingleton<IMongoClient>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var connectionString = config.GetConnectionString("Mongo");
        return new MongoClient(connectionString);
    });

    builder.Services.AddSingleton<IMongoDatabase>(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        return client.GetDatabase("payments_db"); // banco correto
    });

    // Repositórios e Handlers
    builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
    builder.Services.AddScoped<PaymentHandler>();

    // RabbitMQ
    builder.Services.AddSingleton<IMessageBus, RabbitMessageBus>();
}

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

// App
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Endpoints de pagamento
app.MapPaymentEndpoints();

app.MapGet("/health", () => Results.Ok());

app.Run();

// Necessário para testes com WebApplicationFactory<Program>
public partial class Program { }