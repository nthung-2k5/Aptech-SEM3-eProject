using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class DonationCause
{
    [Key]
    public Guid CauseId { get; set; } = Guid.CreateVersion7();

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }

    public ICollection<WelfareProgramme> WelfareProgrammes { get; set; } = new List<WelfareProgramme>();
}
