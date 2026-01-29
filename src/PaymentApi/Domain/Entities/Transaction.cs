using PaymentApi.Domain.Enums;

namespace PaymentApi.Domain.Entities;

public class Transaction
{
    // O construtor força a existência dos dados obrigatórios
    public Transaction(decimal amount, Guid creditCardId)
    {
        if (amount <= 0) 
            throw new DomainException("O valor da transação deve ser maior que zero.");

        Id = Guid.NewGuid();
        Amount = amount;
        CreditCardId = creditCardId;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    // Construtor para o EF Core
    protected Transaction() { }

    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public PaymentStatus Status { get; private set; }
    
    // Chaves Estrangeiras e Propriedades de Navegação
    public Guid CreditCardId { get; private set; }
    public CreditCard? CreditCard { get; private set; }

    // Métodos de Negócio (Comportamento na Entidade)
    public void Approve()
    {
        if (Status != PaymentStatus.Pending) return;
        Status = PaymentStatus.Approved;
    }

    public void Decline()
    {
        if (Status != PaymentStatus.Pending) return;
        Status = PaymentStatus.Declined;
    }

    public void SetCreditCard(CreditCard card)
    {
        CreditCard = card;
        CreditCardId = card.Id;
    }
}

// Pequena classe auxiliar para exceções de domínio
public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
}