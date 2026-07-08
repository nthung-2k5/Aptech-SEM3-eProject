using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

public class WelfareProgramme : IHasCreatedAt
{
    [Key]
    public Guid ProgrammeId { get; set; } = Guid.CreateVersion7();

    public Guid NgoId { get; set; }
    public Ngo Ngo { get; set; } = null!;

    public Guid CauseId { get; set; }
    public DonationCause Cause { get; set; } = null!;

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2048)]
    public string ImageUrl { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    [Precision(18, 2)]
    public decimal? MaxDonation { get; set; }

    [MaxLength(255)]
    public string? Location { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<GalleryImage> GalleryImages { get; set; } = new List<GalleryImage>();
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    
    [NotMapped]
    public IEnumerable<Donation> ValidDonations => Donations.Where(d => d.Status == DonationStatus.Completed);
}
