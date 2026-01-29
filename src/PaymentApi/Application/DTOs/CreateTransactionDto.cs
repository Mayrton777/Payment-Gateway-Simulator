using System.ComponentModel.DataAnnotations;

namespace PaymentApi.Application.DTOs;

public class CreateTransactionDto
{
    [Required(ErrorMessage = "O valor da transação é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "O número do cartão é obrigatório")]
    [CreditCard(ErrorMessage = "Número de cartão inválido")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome do titular é obrigatório")]
    public string HolderName { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de validade é obrigatória (MM/YY)")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$", ErrorMessage = "Formato inválido. Use MM/YY")]
    public string ExpirationDate { get; set; } = string.Empty;

    [Required]
    [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV inválido")]
    public string Cvv { get; set; } = string.Empty;

    // Chave de Idempotência
    public string IdempotencyKey { get; set; } = string.Empty;
}