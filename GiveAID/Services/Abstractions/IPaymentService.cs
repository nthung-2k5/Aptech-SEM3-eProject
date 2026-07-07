using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public record PaymentRequestDto(
    decimal Amount,
    string CardNumber,
    string CardHolderName,
    string ExpiryDate,
    string Cvv,
    string Gateway
);

public interface IPaymentService
{
    Task<Transaction> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken ct = default);
}
