using System.ComponentModel.DataAnnotations;

namespace GiveAID.Dtos;

public record NgoSaveDto(
    [property: Required(ErrorMessage = "NGO Name is required.")]
    [property: MaxLength(200)]
    string Name,

    [property: Required(ErrorMessage = "Description is required.")]
    string Description,

    string? Address,

    [property: Phone(ErrorMessage = "Invalid phone number format.")]
    [property: MaxLength(15)]
    string? PhoneNumber,

    [property: Required]
    [property: Url(ErrorMessage = "Invalid website URL format.")]
    string Website
);
public record NgoSummaryDto(Guid Id, string Name, string Description);

public record NgoDto(Guid Id, string Name, string Description, string? Address, string? PhoneNumber, string Website) : NgoSummaryDto(Id, Name, Description);
