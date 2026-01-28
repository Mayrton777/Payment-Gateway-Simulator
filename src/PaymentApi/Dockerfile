# Estágio de Build com .NET 9.0 SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o resto e compila
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio de Runtime com .NET 9.0 ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PaymentApi.dll"]