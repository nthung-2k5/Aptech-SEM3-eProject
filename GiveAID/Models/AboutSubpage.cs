using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class AboutSubpage
{
    [Key]
    public Guid SubpageId { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string? MediaUrl { get; set; }

    public string? FormattingSettings { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}