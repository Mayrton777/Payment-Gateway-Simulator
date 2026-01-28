using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configura Logs e Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura Redis
// Pega a string de conexão do docker-compose
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

// Configurar SQL Server
// O "TrustServerCertificate=True" é crucial para desenvolvimento local com Docker
var sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(sqlConnection))
{
    // AQUI ENTRARA O DB CONECT NO FUTURO
}

var app = builder.Build();

// Configura Pipeline de Requisição
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();