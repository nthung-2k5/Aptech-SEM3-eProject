using GiveAID.Models;

namespace GiveAID.Dtos;

public record NgoSaveDto(string Name, string Description, string? Address, string? PhoneNumber, string? Website);

public record NgoSummaryDto(Guid Id, string Name, string Description);

public record NgoDto(Guid Id, string Name, string Description, string? Address, string? PhoneNumber, string? Website)
        : NgoSummaryDto(Id, Name, Description);

public static class NgoMapper
{
    extension(Ngo ngo)
    {
        public NgoDto ToDto() =>
            new(ngo.NgoId, ngo.Name, ngo.Description, ngo.Address, ngo.PhoneNumber, ngo.Website);

        public NgoSaveDto ToSaveDto() =>
            new(ngo.Name, ngo.Description, ngo.Address, ngo.PhoneNumber, ngo.Website);
    }

    extension(IQueryable<Ngo> ngos)
    {
        public IQueryable<NgoSummaryDto> ProjectToSummaryDto() =>
                ngos.Select(n => new NgoSummaryDto(n.NgoId, n.Name, n.Description));
        
        public IQueryable<NgoDto> ProjectToDto() =>
                ngos.Select(n => new NgoDto(n.NgoId, n.Name, n.Description, n.Address, n.PhoneNumber, n.Website));
    }

    public static Ngo ToEntity(this NgoSaveDto dto) => new()
    {
        Name = dto.Name,
        Description = dto.Description,
        Address = dto.Address,
        PhoneNumber = dto.PhoneNumber,
        Website = dto.Website
    };
}
