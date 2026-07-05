using System.ComponentModel.DataAnnotations;

namespace GiveAID.Models;

public class Member
{
    [Key]
    public Guid MemberId { get; set; } = Guid.NewGuid();

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    // Personal data
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }

    // Professional data
    public string? Occupation { get; set; }
    public string? Organization { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}