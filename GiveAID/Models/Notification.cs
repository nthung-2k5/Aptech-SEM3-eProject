using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class Notification : IHasCreatedAt
{
    [Key]
    public Guid NotificationId { get; set; } = Guid.CreateVersion7();

    public Guid UserId { get; set; }

    public string Content { get; set; } = string.Empty;

    public bool IsRead { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public User User { get; set; } = null!;
}
