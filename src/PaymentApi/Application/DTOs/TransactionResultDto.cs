namespace PaymentApi.Application.DTOs;

public class TransactionResultDto
{
    public Guid TransactionId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ProcessedAt { get; set; }
}