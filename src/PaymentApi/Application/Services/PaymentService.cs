using System.Text.Json;
using StackExchange.Redis;
using PaymentApi.Application.DTOs;
using PaymentApi.Application.Interfaces;
using PaymentApi.Domain.Entities;
using PaymentApi.Domain.Interfaces;

namespace PaymentApi.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IPaymentRepository repository, 
        IConnectionMultiplexer redis,
        ILogger<PaymentService> logger)
    {
        _repository = repository;
        _redis = redis;
        _logger = logger;
    }

    public async Task<TransactionResultDto> ProcessPaymentAsync(CreateTransactionDto paymentData)
    {
        // Lógica de Idempotência (Redis)
        if (!string.IsNullOrEmpty(paymentData.IdempotencyKey))
        {
            var db = _redis.GetDatabase();
            var cacheKey = $"transaction:idempotency:{paymentData.IdempotencyKey}";
            
            var cachedResult = await db.StringGetAsync(cacheKey);
            if (!cachedResult.IsNullOrEmpty)
            {
                _logger.LogInformation($"Idempotency check: Returning cached result for key {paymentData.IdempotencyKey}");
                return JsonSerializer.Deserialize<TransactionResultDto>(cachedResult!)!;
            }
        }

        // Criação das Entidades
        var card = new CreditCard(
            paymentData.HolderName, 
            paymentData.CardNumber, 
            paymentData.ExpirationDate, 
            paymentData.Cvv
        );

        // Transação vinculada ao Cartão
        var transaction = new Transaction(paymentData.Amount, card.Id);
        transaction.SetCreditCard(card);
        
        // Simulação de Processamento
        if (transaction.Amount > 10000)
        {
            transaction.Decline();
        }
        else
        {
            transaction.Approve();
        }

        // Persistência da Transação
        await _repository.CreateTransactionAsync(transaction);

        // Monta o DTO de resposta
        var result = new TransactionResultDto
        {
            TransactionId = transaction.Id,
            Status = transaction.Status.ToString(),
            Message = transaction.Status == Domain.Enums.PaymentStatus.Approved ? "Transação Aprovada" : "Transação Negada",
            Amount = transaction.Amount,
            ProcessedAt = transaction.CreatedAt
        };

        // Salva no Cache para Idempotência
        if (!string.IsNullOrEmpty(paymentData.IdempotencyKey))
        {
            var db = _redis.GetDatabase();
            var cacheKey = $"transaction:idempotency:{paymentData.IdempotencyKey}";
            
            // Serializa o resultado e salva com expiração de 24 horas
            var jsonResult = JsonSerializer.Serialize(result);
            await db.StringSetAsync(cacheKey, jsonResult, TimeSpan.FromHours(24));
        }

        return result;
    }
}