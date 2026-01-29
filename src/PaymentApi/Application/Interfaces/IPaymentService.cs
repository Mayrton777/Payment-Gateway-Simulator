using PaymentApi.Application.DTOs;

namespace PaymentApi.Application.Interfaces;

public interface IPaymentService
{
    Task<TransactionResultDto> ProcessPaymentAsync(CreateTransactionDto paymentData);
}