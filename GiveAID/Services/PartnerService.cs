using EntityFramework.Exceptions.Common;
using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class PartnerService(AppDbContext dbContext, IImageService imageService) : IPartnerService
{
    public async Task<PagedResult<PartnerDto>> GetPartnersPagedAsync(PartnerQueryParameters query, CancellationToken ct = default)
    {
        var q = dbContext.CorporatePartners.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            q = q.Where(p => p.Name.Contains(query.SearchTerm));
        }

        var totalCount = await q.CountAsync(ct);
        var items = await q.OrderBy(p => p.Name)
                .Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .Select(p => new PartnerDto(p.PartnerId, p.Name, p.LogoUrl, p.WebsiteLink))
                .ToArrayAsync(ct);

        return new PagedResult<PartnerDto>(items, totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<PartnerSummaryDto[]> GetAllPartnersAsync(CancellationToken ct = default)
    {
        return await dbContext.CorporatePartners.AsNoTracking().OrderBy(p => p.Name)
                .Select(p => new PartnerSummaryDto(p.PartnerId, p.Name, p.LogoUrl)).ToArrayAsync(ct);
    }

    public async Task<PartnerDto[]> GetAllPartnerDtosAsync(CancellationToken ct = default)
    {
        return await dbContext.CorporatePartners.AsNoTracking().OrderBy(p => p.Name)
                .Select(p => new PartnerDto(p.PartnerId, p.Name, p.LogoUrl, p.WebsiteLink))
                .ToArrayAsync(ct);
    }

    public async Task<PartnerDto?> GetPartnerByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.CorporatePartners.AsNoTracking().Where(p => p.PartnerId == id)
                .Select(p => new PartnerDto(p.PartnerId, p.Name, p.LogoUrl, p.WebsiteLink))
                .FirstOrDefaultAsync(ct);
    }

    public async Task<PartnerDto> CreatePartnerAsync(PartnerSaveDto dto, CancellationToken ct = default)
    {
        try
        {
            var entity = dto.ToEntity();

            dbContext.CorporatePartners.Add(entity);
            await dbContext.SaveChangesAsync(ct);

            return entity.ToDto();
        }
        catch (UniqueConstraintException) { throw new DuplicateException(nameof(dto.Name)); }
    }

    public async Task UpdatePartnerAsync(Guid id, PartnerSaveDto dto, CancellationToken ct = default)
    {
        try
        {
            int result = await dbContext.CorporatePartners.Where(p => p.PartnerId == id).ExecuteUpdateAsync(
                s => s.SetProperty(p => p.Name, dto.Name).SetProperty(p => p.LogoUrl, dto.LogoUrl)
                        .SetProperty(p => p.WebsiteLink, dto.WebsiteLink),
                ct);

            if (result == 0) { throw new NotFoundException(); }
        }
        catch (UniqueConstraintException) { throw new DuplicateException(nameof(dto.Name)); }
    }

    public async Task DeletePartnerAsync(Guid id, CancellationToken ct = default)
    {
        string? imageUrl = await dbContext.CorporatePartners.AsNoTracking().Where(i => i.PartnerId == id).Select(i => i.LogoUrl).FirstOrDefaultAsync(ct);

        if (!string.IsNullOrEmpty(imageUrl)) { await imageService.DeleteImageAsync(new Uri(imageUrl)); }
        
        await dbContext.CorporatePartners.Where(p => p.PartnerId == id).ExecuteDeleteAsync(ct);
    }
}
