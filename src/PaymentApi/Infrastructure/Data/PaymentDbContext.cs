using Microsoft.EntityFrameworkCore;
using PaymentApi.Domain.Entities;

namespace PaymentApi.Infrastructure.Data;

public class PaymentDbContext : DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CreditCard> CreditCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuração Fluent API
        
        // Mapeamento de Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)").IsRequired();
            
            // Relacionamento 1:1 ou 1:N
            entity.HasOne(e => e.CreditCard)
                  .WithMany(c => c.Transactions)
                  .HasForeignKey(e => e.CreditCardId);
        });

        // Mapeamento de CreditCard
        modelBuilder.Entity<CreditCard>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.HolderName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CardToken).IsRequired().HasMaxLength(50);
            entity.Property(e => e.MaskedNumber).IsRequired().HasMaxLength(4);
        });

        base.OnModelCreating(modelBuilder);
    }
}