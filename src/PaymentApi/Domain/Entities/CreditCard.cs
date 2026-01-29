namespace PaymentApi.Domain.Entities;

public class CreditCard
{
    public CreditCard(string holderName, string number, string expirationDate, string cvv)
    {
        Id = Guid.NewGuid();
        HolderName = holderName;
        CardToken = Guid.NewGuid().ToString();
        MaskedNumber = number.Length > 4 ? number.Substring(number.Length - 4) : number;
        ExpirationDate = expirationDate;
    }
    
    // Construtor para o EF Core
    protected CreditCard()
    {
        HolderName = null!;
        CardToken = null!;
        MaskedNumber = null!;
        ExpirationDate = null!;
    }

    public Guid Id { get; private set; }
    public string HolderName { get; private set; }
    public string CardToken { get; private set; }
    public string MaskedNumber { get; private set; }
    public string ExpirationDate { get; private set; }
    
    // Relação 1:N
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
}