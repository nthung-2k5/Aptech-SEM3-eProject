using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

public class Donation: IHasCreatedAt
{
    [Key]
    public Guid DonationId { get; set; } = Guid.CreateVersion7();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid? NgoId { get; set; }
    public Ngo? Ngo { get; set; }

    public Guid? ProgrammeId { get; set; }
    public WelfareProgramme? Programme { get; set; }

    public Guid? CauseId { get; set; }
    public DonationCause? Cause { get; set; }

    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = null!;

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public DonationStatus Status { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum DonationStatus
{
    Completed,
    Void
}
