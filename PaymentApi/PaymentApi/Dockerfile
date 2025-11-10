# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PaymentApi/PaymentApi.csproj", "PaymentApi/"]
COPY ["CrossCuting/CrossCuting.csproj", "CrossCuting/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["PaymentApiTests/PaymentApiTests.csproj", "PaymentApiTests/"]
RUN dotnet restore "./PaymentApi/PaymentApi.csproj"
COPY . .
WORKDIR "/src/PaymentApi"
RUN dotnet build "./PaymentApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PaymentApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentApi.dll"]