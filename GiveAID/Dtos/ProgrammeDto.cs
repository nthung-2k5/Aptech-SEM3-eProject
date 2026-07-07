using System.ComponentModel.DataAnnotations;

namespace GiveAID.Dtos;

public record ProgrammeSaveDto(
    [property: Required(ErrorMessage = "NGO is required.")]
    Guid NgoId,

    [property: Required(ErrorMessage = "Cause is required.")]
    Guid CauseId,

    [property: Required(ErrorMessage = "Programme name is required.")]
    [property: MaxLength(255, ErrorMessage = "Name cannot exceed 255 characters.")]
    string Name,

    [property: Required(ErrorMessage = "Description is required.")]
    string Description,

    [property: Required(ErrorMessage = "Image URL is required.")]
    [property: Url(ErrorMessage = "Invalid image URL format.")]
    string ImageUrl,

    [property: Required(ErrorMessage = "Start time is required.")]
    DateTimeOffset StartTime,

    DateTimeOffset? EndTime,

    [property: Range(0, double.MaxValue, ErrorMessage = "Target amount must be a positive number.")]
    decimal? MaxDonation,

    [property: MaxLength(500, ErrorMessage = "Location cannot exceed 500 characters.")]
    string? Location
);

public record ProgrammeSummaryDto(
    Guid Id,
    string Name,
    string Cause,
    string Ngo,
    string ImageUrl,
    DateTime StartDate,
    DateTime EndDate,
    long DonationCount,
    decimal? TargetAmount,
    decimal RaisedAmount
);

public record ProgrammeDto(
    Guid Id,
    string Name,
    string Cause,
    string Ngo,
    string ImageUrl,
    DateTime StartDate,
    DateTime EndDate,
    long DonationCount,
    decimal? TargetAmount,
    decimal RaisedAmount,
    string Description,
    string OrganizerInfo
) : ProgrammeSummaryDto(
    Id,
    Name,
    Cause,
    Ngo,
    ImageUrl,
    StartDate,
    EndDate,
    DonationCount,
    TargetAmount,
    RaisedAmount
);