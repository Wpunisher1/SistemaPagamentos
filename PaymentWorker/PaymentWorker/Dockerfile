# Imagem base para runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Fase de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PaymentWorker/PaymentWorker.csproj", "PaymentWorker/"]
COPY ["CrossCuting/CrossCuting.csproj", "CrossCuting/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]
RUN dotnet restore "./PaymentWorker/PaymentWorker.csproj"
COPY . .
WORKDIR "/src/PaymentWorker"
RUN dotnet build "./PaymentWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentWorker.dll"]