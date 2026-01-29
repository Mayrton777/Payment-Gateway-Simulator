using Microsoft.EntityFrameworkCore;
using PaymentApi.Domain.Entities;
using PaymentApi.Domain.Interfaces;
using PaymentApi.Infrastructure.Data;

namespace PaymentApi.Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    // Injeção de Dependência do DbContext
    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task CreateTransactionAsync(Transaction transaction)
    {
        // Adiciona ao contexto
        await _context.Transactions.AddAsync(transaction);
        
        // Persiste no banco de dados
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        // Busca no banco incluindo os dados do Cartão
        return await _context.Transactions
            .Include(t => t.CreditCard)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}