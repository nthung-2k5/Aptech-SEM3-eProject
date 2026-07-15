using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class NgoService(AppDbContext dbContext) : INgoService
{
    public async Task<PagedResult<NgoSummaryDto>> GetNgosPagedAsync(NgoQueryParameters query, CancellationToken ct = default)
    {
        var q = dbContext.ActiveNgos.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            q = q.Where(n => n.Name.Contains(query.SearchTerm) || n.Description.Contains(query.SearchTerm));
        }

        var totalCount = await q.CountAsync(ct);
        var items = await q.OrderByDescending(n => n.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .ProjectToSummaryDto().ToArrayAsync(ct);

        return new PagedResult<NgoSummaryDto>(items, totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<NgoSummaryDto[]> GetAllNgosAsync(CancellationToken ct = default)
    {
        return await dbContext.ActiveNgos.AsNoTracking().OrderByDescending(n => n.CreatedAt).ProjectToSummaryDto()
                .ToArrayAsync(ct);
    }

    public async Task<NgoDto?> GetNgoByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.ActiveNgos.AsNoTracking().Where(n => n.NgoId == id).ProjectToDto()
                .FirstOrDefaultAsync(ct);
    }

    public async Task<NgoDto> CreateNgoAsync(NgoSaveDto dto, CancellationToken ct = default)
    {
        // Reject if an active NGO with the same name already exists
        if (await dbContext.ActiveNgos.AnyAsync(n => n.Name == dto.Name, ct))
        {
            throw new DuplicateException(nameof(dto.Name));
        }

        var entity = dto.ToEntity();

        await dbContext.Ngos.AddAsync(entity, ct);
        await dbContext.NgoPartners.AddRangeAsync(dto.PartnersId.Select(partnerId => new NgoPartner
        {
            NgoId = entity.NgoId,
            PartnerId = partnerId
        }), ct);

        await dbContext.SaveChangesAsync(ct);

        return await dbContext.ActiveNgos.AsNoTracking()
                .Where(n => n.NgoId == entity.NgoId)
                .ProjectToDto()
                .FirstAsync(ct);
    }

    public async Task UpdateNgoAsync(Guid id, NgoSaveDto dto, CancellationToken ct = default)
    {
        // Reject if another active NGO already uses the same name
        if (await dbContext.ActiveNgos.AnyAsync(n => n.Name == dto.Name && n.NgoId != id, ct))
        {
            throw new DuplicateException(nameof(dto.Name));
        }

        var entity = await dbContext.Ngos.FindAsync([id], ct);

        if (entity == null || entity.IsDeleted) { throw new NotFoundException(); }

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Address = dto.Address;
        entity.PhoneNumber = dto.PhoneNumber;
        entity.Website = dto.Website;

        await dbContext.NgoPartners.Where(p => p.NgoId == id).ExecuteDeleteAsync(ct);
        await dbContext.NgoPartners.AddRangeAsync(dto.PartnersId.Select(partnerId => new NgoPartner
        {
            NgoId = entity.NgoId,
            PartnerId = partnerId
        }), ct);

        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteNgoAsync(Guid id, CancellationToken ct = default)
    {
        await dbContext.ActiveNgos.Where(n => n.NgoId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsDeleted, true), ct);
    }
}
