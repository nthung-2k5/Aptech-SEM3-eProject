using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class PartnerService(AppDbContext dbContext) : IPartnerService
{
    public async Task<PartnerSummaryDto[]> GetAllPartnersAsync(CancellationToken ct = default)
    {
        return await dbContext.CorporatePartners
            .AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new PartnerSummaryDto(p.PartnerId, p.Name, p.LogoUrl))
            .ToArrayAsync(ct);
    }

    public async Task<PartnerDto?> GetPartnerByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.CorporatePartners
            .AsNoTracking()
            .Where(p => p.PartnerId == id)
            .Select(p => new PartnerDto(
                p.PartnerId,
                p.Name,
                p.LogoUrl,
                p.Description,
                p.WebsiteLink))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<PartnerDto?> CreatePartnerAsync(PartnerSaveDto partner, CancellationToken ct = default)
    {
        // CorporatePartner.Name has a unique DB index — guard here to return null instead of throwing
        bool exists = await dbContext.CorporatePartners.AnyAsync(p => p.Name == partner.Name, ct);
        if (exists) return null;

        var entity = new CorporatePartner
        {
            Name = partner.Name,
            LogoUrl = partner.LogoUrl,
            Description = partner.Description,
            WebsiteLink = partner.WebsiteLink,
        };

        dbContext.CorporatePartners.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        return new PartnerDto(
            entity.PartnerId,
            entity.Name,
            entity.LogoUrl,
            entity.Description,
            entity.WebsiteLink);
    }

    public async Task<bool> UpdatePartnerAsync(Guid id, PartnerSaveDto partner, CancellationToken ct = default)
    {
        // Guard against unique-index violation: another partner already uses the new name
        bool nameConflict = await dbContext.CorporatePartners
            .AnyAsync(p => p.Name == partner.Name && p.PartnerId != id, ct);
        if (nameConflict) return false;

        var entity = await dbContext.CorporatePartners
            .FirstOrDefaultAsync(p => p.PartnerId == id, ct);

        if (entity == null) return false;

        entity.Name = partner.Name;
        entity.LogoUrl = partner.LogoUrl;
        entity.Description = partner.Description;
        entity.WebsiteLink = partner.WebsiteLink;

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeletePartnerAsync(Guid id, CancellationToken ct = default)
    {
        // Hard delete — CorporatePartner has no IsDeleted field
        return await dbContext.CorporatePartners
            .Where(p => p.PartnerId == id)
            .ExecuteDeleteAsync(ct) > 0;
    }
}
