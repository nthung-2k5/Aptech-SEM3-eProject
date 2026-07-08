using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

public class Transaction
{
    [Key]
    public Guid TransactionId { get; set; } = Guid.CreateVersion7();

    [MaxLength(50)]
    public string Gateway { get; set; } = string.Empty;

    [MaxLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [MaxLength(50)]
    public string ReferenceCode { get; set; } = string.Empty;

    public DateTimeOffset TransactionTime { get; set; } = DateTimeOffset.UtcNow;
    
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
