namespace GiveAID.Dtos;

public record NgoSaveDto(string Name, string Description, string? Address, string? PhoneNumber, string Website);

public record NgoSummaryDto(Guid Id, string Name, string Description);

public record NgoDto(Guid Id, string Name, string Description, string? Address, string? PhoneNumber, string Website);