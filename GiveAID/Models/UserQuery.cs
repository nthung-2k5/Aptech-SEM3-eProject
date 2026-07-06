using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class UserQuery : IHasCreatedAt
{
    [Key]
    public Guid QueryId { get; set; } = Guid.CreateVersion7();

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [MaxLength(255)]
    public string Subject { get; set; } = string.Empty;

    public string MessageText { get; set; } = string.Empty;

    public string? ReplyText { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
