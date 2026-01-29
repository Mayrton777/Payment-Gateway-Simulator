using PaymentApi.Domain.Entities;

namespace PaymentApi.Domain.Interfaces;

public interface IPaymentRepository
{
    // Método para salvar uma nova transação
    Task CreateTransactionAsync(Transaction transaction);

    // Método para buscar uma transação pelo ID
    Task<Transaction?> GetTransactionByIdAsync(Guid id);
}