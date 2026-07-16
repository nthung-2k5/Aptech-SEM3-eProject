using GiveAID.Models;

namespace GiveAID.Dtos;

public record PartnerSaveDto(string Name, string LogoUrl, string WebsiteLink);

public record PartnerSummaryDto(Guid Id, string Name, string LogoUrl);

public record PartnerDto(Guid Id, string Name, string LogoUrl, string WebsiteLink)
        : PartnerSummaryDto(Id, Name, LogoUrl);

public static class PartnerMapper
{
    extension(CorporatePartner partner)
    {
        public PartnerDto ToDto() => new(partner.PartnerId, partner.Name, partner.LogoUrl, partner.WebsiteLink);

        public PartnerSaveDto ToSaveDto() => new(partner.Name, partner.LogoUrl, partner.WebsiteLink);
    }

    public static IQueryable<PartnerSummaryDto> ProjectToSummaryDto(this IQueryable<CorporatePartner> partners) =>
            partners.Select(p => new PartnerSummaryDto(p.PartnerId, p.Name, p.LogoUrl));

    public static CorporatePartner ToEntity(this PartnerSaveDto dto) =>
            new()
            {
                Name = dto.Name,
                LogoUrl = dto.LogoUrl,
                WebsiteLink = dto.WebsiteLink
            };
}
