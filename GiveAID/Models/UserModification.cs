using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class UserModification : IHasCreatedAt
{
    [Key]
    public Guid ModificationId { get; set; } = Guid.CreateVersion7();

    public Guid SubpageId { get; set; }
    public AboutUsSubpage Subpage { get; set; } = null!;

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public string HtmlContent { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
