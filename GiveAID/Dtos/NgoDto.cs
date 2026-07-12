using GiveAID.Models;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Dtos;

public record NgoSaveDto(string Name, string Description, string? Address, string? PhoneNumber, string? Website, Guid[] PartnersId);

public record NgoSummaryDto(Guid Id, string Name, string Description);

public record NgoDto(Guid Id, string Name, string Description, string? Address, string? PhoneNumber, string? Website, PartnerDto[] Partners)
        : NgoSummaryDto(Id, Name, Description);

public static class NgoMapper
{
    extension(Ngo ngo)
    {
        public NgoDto ToDto() =>
            new(ngo.NgoId, ngo.Name, ngo.Description, ngo.Address, ngo.PhoneNumber, ngo.Website, ngo.NgoPartners.Select(p => p.Partner.ToDto()).ToArray());

        public NgoSaveDto ToSaveDto() =>
            new(ngo.Name, ngo.Description, ngo.Address, ngo.PhoneNumber, ngo.Website, ngo.NgoPartners.Select(p => p.PartnerId).ToArray());
    }

    extension(IQueryable<Ngo> ngos)
    {
        public IQueryable<NgoSummaryDto> ProjectToSummaryDto() =>
                ngos.Select(n => new NgoSummaryDto(n.NgoId, n.Name, n.Description));

        public IQueryable<NgoDto> ProjectToDto() =>
                ngos.Include(n => n.NgoPartners)
                        .ThenInclude(p => p.Partner)
                        .Select(n => new NgoDto(n.NgoId, n.Name, n.Description, n.Address, n.PhoneNumber, n.Website, n.NgoPartners.Select(p => p.Partner.ToDto()).ToArray()));
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
