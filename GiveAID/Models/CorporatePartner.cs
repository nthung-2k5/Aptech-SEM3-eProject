using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

[Index(nameof(Name), IsUnique = true)]
public class CorporatePartner
{
    [Key]
    public Guid PartnerId { get; set; } = Guid.CreateVersion7();

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string LogoUrl { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [MaxLength(1024)]
    public string WebsiteLink { get; set; } = string.Empty;

    public ICollection<NgoPartner> NgoPartners { get; set; } = new List<NgoPartner>();
}
