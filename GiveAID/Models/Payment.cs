namespace GiveAID.Models;

// It holds credit card details
public class Payment
{
    public Guid UserId { get; set; }
    
    // Credit card details
    public string CardNumber { get; set; }
    public string CardHolderName { get; set; }
    public string ExpiryDate { get; set; }
    public string CVV { get; set; }

    public decimal Amount { get; set; }
}