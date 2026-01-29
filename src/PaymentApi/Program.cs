using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using PaymentApi.Application.Interfaces;
using PaymentApi.Application.Services;
using PaymentApi.Domain.Interfaces;
using PaymentApi.Infrastructure.Data;
using PaymentApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configura Logs e Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura Redis
// Pega a string de conexão do docker-compose
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect($"{redisConnection},abortConnect=false"));

// Configurar SQL Server
var sqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(sqlConnection));

// Injeção de Dependência dos Repositórios
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

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