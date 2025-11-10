using MongoDB.Driver;
using BalanceApi.Config;
using BalanceApi.Domain.Handlers;
using BalanceApi.Domain.Repositories;
using BalanceApi.Infra.Data;
using BalanceApi.Presentation.Endpoints;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configurações do MongoDB via appsettings.json
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("Mongo"));

// Registra MongoClient com ConnectionString
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Registra banco de dados com DatabaseName
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(settings.DatabaseName);
});

// Repositórios e Handlers
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddScoped<BalanceHandler>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

// Endpoints
app.MapBalanceEndpoints();

// Ajuste no endpoint de health para retornar texto puro
app.MapGet("/", () => "ok");

app.Run();

// Necessário para testes com WebApplicationFactory
public partial class Program { }