using GiveAID.Models;

namespace GiveAID.Dtos;

public record DonationCauseSaveDto(string Name);

public record DonationCauseDto(Guid Id, string Name);

public static class DonationCauseMapper
{
    public static DonationCauseDto ToDto(this DonationCause cause) => new(cause.CauseId, cause.Name);
    
    public static DonationCause ToEntity(this DonationCauseSaveDto dto) => new()
    {
        Name = dto.Name
    };
    
    public static IQueryable<DonationCauseDto> ProjectToDto(this IQueryable<DonationCause> causes) =>
        causes.Select(cause => new DonationCauseDto(cause.CauseId, cause.Name));
}
