using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models;

[Index(nameof(Email), IsUnique = true)]
public class User
{
    public Guid UserId { get; set; } = Guid.CreateVersion7();
    
    [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
    public string FullName { get; set; } = string.Empty;
    
    [EmailAddress(ErrorMessage = "The email address is not valid.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    [MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
    public string Address { get; set; } = string.Empty;
    
    // Must be a Vietnamese valid phone number
    [RegularExpression(@"^(?:\+84|0)(?:3[2-9]|5[2|5|6|8|9]|7[0|6-9]|8[1-9]|9[0-4|6-9])[0-9]{7}$", ErrorMessage = "The phone number is not valid.")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Occupation { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum UserRole
{
    Admin,
    Member
}
