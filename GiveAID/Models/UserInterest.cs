using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

[PrimaryKey(nameof(UserId), nameof(NgoId))]
public class UserInterest
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid NgoId { get; set; }
    public Ngo Ngo { get; set; } = null!;
}
