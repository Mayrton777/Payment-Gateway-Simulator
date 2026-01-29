using Microsoft.AspNetCore.Mvc;
using PaymentApi.Application.DTOs;
using PaymentApi.Application.Interfaces;

namespace PaymentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ProcessPayment(
        [FromBody] CreateTransactionDto paymentData,
        [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        // Se o cliente mandou a chave no Header, injetamos no DTO
        if (!string.IsNullOrEmpty(idempotencyKey))
        {
            paymentData.IdempotencyKey = idempotencyKey;
        }

        try
        {
            var result = await _paymentService.ProcessPaymentAsync(paymentData);
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}