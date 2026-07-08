using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class Ngo : IHasCreatedAt
{
    [Key]
    public Guid NgoId { get; set; } = Guid.CreateVersion7();

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Address { get; set; }

    [MaxLength(11)]
    public string? PhoneNumber { get; set; }

    [MaxLength(1024)]
    public string? Website { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<WelfareProgramme> WelfareProgrammes { get; set; } = new List<WelfareProgramme>();
    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
    public ICollection<NgoPartner> NgoPartners { get; set; } = new List<NgoPartner>();
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
