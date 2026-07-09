using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class User : IHasCreatedAt
{
    public Guid UserId { get; set; } = Guid.CreateVersion7();
    
    [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
    public string FullName { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "The email address is not valid.")]
    [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateOnly DateOfBirth { get; set; }
    
    [MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
    public string Address { get; set; } = string.Empty;
    
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string Occupation { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
    public ICollection<UserQuery> UserQueries { get; set; } = new List<UserQuery>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<UserModification> UserModifications { get; set; } = new List<UserModification>();
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
}

public enum UserRole
{
    Admin,
    Member
}
