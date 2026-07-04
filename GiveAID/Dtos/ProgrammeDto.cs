namespace GiveAID.Dtos;

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
): ProgrammeSummaryDto(
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