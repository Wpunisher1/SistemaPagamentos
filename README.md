# 💸 SistemaPagamentos

Projeto distribuído para processamento de pagamentos de forma assíncrona, confiável e escalável. Utiliza mensageria com RabbitMQ, persistência com MongoDB e comunicação entre serviços via HTTP.



---

## ⚙️ Tecnologias utilizadas

- .NET / C# – desenvolvimento dos serviços  
- RabbitMQ – mensageria entre API e Worker  
- MongoDB – persistência dos pagamentos  
- Docker – infraestrutura portátil  
- xUnit + Moq – testes unitários  
- VS / VS Code / Rider – IDEs recomendadas  

---

## 🚀 Como rodar o projeto

### 1. Subir MongoDB e RabbitMQ com Docker:

Painel do RabbitMQ: http://localhost:15672
Usuário: admin | Senha: admin

2. Configurar appsettings.json
Em cada projeto (PaymentApi, PaymentWorker, BalanceApi), configure:

"MongoSettings": {
  "ConnectionString": "mongodb://localhost:27017",
  "Database": "payment-db"
},
"Rabbit": {
  "HostName": "localhost",
  "UserName": "admin",
  "Password": "admin",
  "QueueName": "payments-queue"
},
"BalanceApi": {
  "BaseAddress": "http://localhost:5000"
}

3. Rodar Serviços:

dotnet run --project PaymentApi
dotnet run --project BalanceApi
dotnet run --project PaymentWorker

Fluxos de uso
🔹 Criar usuário no BalanceApi
Antes de processar pagamentos, crie um usuário com saldo inicial:

POST http://localhost:8082/api/v1/balance/update
Content-Type: application/json

{
  "accountId": "acc-06",
  "amount": 1000,
  "operation": "credit"
}

🔹 Processar pagamento
Envie um pagamento pela PaymentApi:

POST http://localhost:8080/api/v1/payments
Content-Type: application/json

{
  "PaymentId": "pay001",
  "AccountId": "acc123",
  "Amount": 100.00,
  "Operation": "processing"
}

🔹 Confirmar pagamento
Para confirmar pagamento:

http://localhost:8080/api/v1/payment/confirm

{
  "paymentId": "69113707e520f31ae1cf0fb3"
}

🔹 Cancelar pagamento
Para simular um cancelamento:

POST http://localhost:8080/api/v1/payments/cancel
Content-Type: application/json

{
  "PaymentId": "pay001",
  "AccountId": "acc123",
  "Reason": "Cliente desistiu da compra"
}

Consultar Saldo:

GET http://localhost:8082/api/v1/balance/acc123

{
  "AccountId": "acc123",
  "Balance": 400.00
}

Autor
Projeto criado por William Fonseca
Local: São Paulo, Brasil
Data: Novembro de 2025




```bash
docker run -d -p 27017:27017 --name mongo mongo
docker run -d -p 5672:5672 -p 15672:15672 --name rabbitmq rabbitmq:3-management
