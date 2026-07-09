using GiveAID.Models;

namespace GiveAID.Dtos;

public record ProgrammeDto(
    Guid Id,
    string Name,
    string Cause,
    Guid NgoId,
    string NgoName,
    string NgoDescription,
    string ImageUrl,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate,
    long DonationCount,
    decimal? TargetAmount,
    decimal RaisedAmount,
    string Description,
    string? Location
);

public record ProgrammeSaveDto(
    Guid NgoId,
    Guid CauseId,
    string Name,
    string ImageUrl,
    string Description,
    DateTimeOffset StartTime,
    DateTimeOffset? EndTime,
    decimal? MaxDonation,
    string? Location
);

public static class ProgrammeMapper
{
    public static ProgrammeDto ToDto(this WelfareProgramme programme) => new(
        programme.ProgrammeId,
        programme.Name,
        programme.Cause.Name,
        programme.NgoId,
        programme.Ngo.Name,
        programme.Ngo.Description,
        programme.ImageUrl,
        programme.StartTime,
        programme.EndTime,
        programme.ValidDonations.Count(),
        programme.MaxDonation,
        programme.ValidDonations.Sum(d => d.Amount),
        programme.Description,
        programme.Location
    );
    
    public static IQueryable<ProgrammeDto> ProjectToDto(this IQueryable<WelfareProgramme> programmes) =>
        programmes.Select(p => new ProgrammeDto(
            p.ProgrammeId,
            p.Name,
            p.Cause.Name,
            p.NgoId,
            p.Ngo.Name,
            p.Ngo.Description,
            p.ImageUrl,
            p.StartTime,
            p.EndTime,
            p.Donations.Count(d => d.Status == DonationStatus.Completed),
            p.MaxDonation,
            p.Donations.Where(d => d.Status == DonationStatus.Completed).Sum(d => d.Amount),
            p.Description,
            p.Location
        ));
    
    public static WelfareProgramme ToEntity(this ProgrammeSaveDto dto) => new()
    {
        NgoId = dto.NgoId,
        CauseId = dto.CauseId,
        Name = dto.Name,
        ImageUrl = dto.ImageUrl,
        Description = dto.Description,
        StartTime = dto.StartTime,
        EndTime = dto.EndTime,
        MaxDonation = dto.MaxDonation,
        Location = dto.Location
    };
}
