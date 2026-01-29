using System.Text.Json;
using Moq;
using Xunit;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using PaymentApi.Application.DTOs;
using PaymentApi.Application.Services;
using PaymentApi.Domain.Entities;
using PaymentApi.Domain.Enums;
using PaymentApi.Domain.Interfaces;

namespace PaymentApi.Tests.Application.Services;

public class PaymentServiceTests
{
    // Mocks das dependências do PaymentService
    private readonly Mock<IPaymentRepository> _repositoryMock;
    private readonly Mock<IConnectionMultiplexer> _redisMock;
    private readonly Mock<IDatabase> _redisDbMock;
    private readonly Mock<ILogger<PaymentService>> _loggerMock;
    
    // O Serviço que será testado
    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        // Arrange dos Mocks
        _repositoryMock = new Mock<IPaymentRepository>();
        _redisMock = new Mock<IConnectionMultiplexer>();
        _redisDbMock = new Mock<IDatabase>();
        _loggerMock = new Mock<ILogger<PaymentService>>();

        // Configura o Mock do Redis para devolver o banco de dados falso
        _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                  .Returns(_redisDbMock.Object);

        // Instancia o serviço injetando os mocks no lugar das coisas reais
        _paymentService = new PaymentService(
            _repositoryMock.Object,
            _redisMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task ProcessPayment_ShouldDecline_WhenAmountIsGreaterThan10000()
    {
        var dto = new CreateTransactionDto
        {
            Amount = 15000,
            CardNumber = "4111111111111111",
            HolderName = "Teste Unitario",
            ExpirationDate = "12/30",
            Cvv = "123"
        };

        var result = await _paymentService.ProcessPaymentAsync(dto);

        Assert.Equal("Declined", result.Status);
        Assert.Equal("Transação Negada", result.Message);

        _repositoryMock.Verify(r => r.CreateTransactionAsync(It.IsAny<Transaction>()), Times.Once);
    }

    [Fact]
    public async Task ProcessPayment_ShouldApprove_WhenAmountIsLow()
    {
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            CardNumber = "4111111111111111",
            HolderName = "Teste Aprovado",
            ExpirationDate = "12/30",
            Cvv = "123"
        };

        var result = await _paymentService.ProcessPaymentAsync(dto);

        Assert.Equal("Approved", result.Status);
        Assert.Equal("Transação Aprovada", result.Message);
    }

    [Fact]
    public async Task ProcessPayment_ShouldReturnCachedResult_WhenIdempotencyKeyExists()
    {
        var idempotencyKey = "chave-duplicada-123";
        var dto = new CreateTransactionDto
        {
            Amount = 100,
            IdempotencyKey = idempotencyKey
        };

        var cachedResult = new TransactionResultDto
        {
            TransactionId = Guid.NewGuid(),
            Status = "Approved",
            Message = "Retornado do Cache"
        };
        var jsonResult = JsonSerializer.Serialize(cachedResult);

        _redisDbMock.Setup(db => db.StringGetAsync(
            It.Is<RedisKey>(k => k == $"transaction:idempotency:{idempotencyKey}"), 
            It.IsAny<CommandFlags>()))
            .ReturnsAsync(jsonResult);

        var result = await _paymentService.ProcessPaymentAsync(dto);

        Assert.Equal("Retornado do Cache", result.Message);
        
        _repositoryMock.Verify(r => r.CreateTransactionAsync(It.IsAny<Transaction>()), Times.Never);
    }
}