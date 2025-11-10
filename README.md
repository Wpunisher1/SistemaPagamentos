# 💸 SistemaPagamentos

Projeto distribuído para processamento de pagamentos de forma assíncrona, confiável e escalável. Utiliza mensageria com RabbitMQ, persistência com MongoDB e comunicação entre serviços via HTTP.

---

## 🧩 Estrutura do projeto
SistemaPagamentos/ ├── PaymentApi/
# API que publica mensagens de pagamento ├── PaymentWorker/
# Worker que consome mensagens e processa pagamentos ├── BalanceApi/
# API que simula atualização de saldo ├── docker-compose.yml  
# (opcional) para orquestrar serviços ├── README.md           
# documentação do projeto


---

## ⚙️ Tecnologias utilizadas

- .NET / C# – desenvolvimento dos serviços  
- RabbitMQ – mensageria entre API e Worker  
- MongoDB – persistência dos pagamentos  
- Docker – infraestrutura portátil  
- xUnit + Moq – testes unitários  
- VS Code / Rider – IDEs recomendadas  

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

POST http://localhost:5000/api/balance/update
Content-Type: application/json

{
  "accountId": "acc-06",
  "amount": 1000,
  "operation": "credit"
}

🔹 Processar pagamento
Envie um pagamento pela PaymentApi:

POST http://localhost:5001/api/payments
Content-Type: application/json

{
  "PaymentId": "pay001",
  "AccountId": "acc123",
  "Amount": 100.00,
  "Operation": "processing"
}

🔹 Cancelar pagamento
Para simular um cancelamento:

POST http://localhost:5001/api/payments/cancel
Content-Type: application/json

{
  "PaymentId": "pay001",
  "AccountId": "acc123",
  "Reason": "Cliente desistiu da compra"
}

Consultar Saldo:

GET http://localhost:5000/api/balance/acc123

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
