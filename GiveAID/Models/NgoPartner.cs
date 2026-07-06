using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

[PrimaryKey(nameof(NgoId), nameof(PartnerId))]
public class NgoPartner
{
    public Guid NgoId { get; set; }
    public Ngo Ngo { get; set; } = null!;

    public Guid PartnerId { get; set; }
    public CorporatePartner Partner { get; set; } = null!;
}
