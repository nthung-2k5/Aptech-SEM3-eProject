using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class FakePaymentService(AppDbContext dbContext) : IPaymentService
{
    public async Task<Transaction> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken ct = default)
    {
        // Simulate a delay for processing
        await Task.Delay(1000, ct);

        // Fake payment always succeeds.
        var transaction = new Transaction
        {
            Gateway = request.Gateway,
            AccountNumber = request.CardNumber,
            Content = $"Donation via {request.Gateway}",
            Amount = request.Amount,
            ReferenceCode = Guid.NewGuid().ToString("N")[..10].ToUpper()
        };

        await dbContext.Transactions.AddAsync(transaction, ct);
        await dbContext.SaveChangesAsync(ct);

        return transaction;
    }
}
