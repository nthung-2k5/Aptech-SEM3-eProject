using System.Security.Claims;
using EntityFramework.Exceptions.Common;
using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class AboutUsSubpageService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        : IAboutUsSubpageService
{
    private Guid? GetCurrentUserId()
    {
        string? idClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(idClaim, out var id) ? id : null;
    }

    public async Task<AboutUsSubpageSummaryDto[]> ListSubpagesAsync(CancellationToken ct = default)
    {
        return await dbContext.AboutUsSubpages.AsNoTracking()
                .Include(s => s.UserModifications.OrderByDescending(o => o.CreatedAt).Take(1)).ProjectToSummaryDto()
                .ToArrayAsync(ct);
    }

    public async Task<AboutUsSubpageDto?> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        return await dbContext.AboutUsSubpages.AsNoTracking()
                .Include(s => s.UserModifications.OrderByDescending(o => o.CreatedAt).Take(1))
                .Where(s => s.Slug == slug).ProjectToDto().FirstOrDefaultAsync(ct);
    }

    public async Task AddSubpageAsync(AboutUsSubpageDto dto, CancellationToken ct = default)
    {
        if (await dbContext.AboutUsSubpages.AnyAsync(s => s.Slug == dto.Slug, ct))
        {
            throw new DuplicateException(nameof(dto.Slug));
        }

        if (await dbContext.AboutUsSubpages.AnyAsync(s => s.Title == dto.Title, ct))
        {
            throw new DuplicateException(nameof(dto.Title));
        }

        var entity = dto.MapToEntity();
        await dbContext.AboutUsSubpages.AddAsync(entity, ct);

        entity.UserModifications.Add(
            new UserModification
            {
                HtmlContent = dto.HtmlContent,
                UserId = GetCurrentUserId(),
                Subpage = entity
            });

        await dbContext.AboutUsSubpages.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateSubpageAsync(AboutUsSubpageDto dto, CancellationToken ct = default)
    {
        try
        {
            int result = await dbContext.AboutUsSubpages.Where(p => p.Slug == dto.Slug).ExecuteUpdateAsync(
                s => s.SetProperty(p => p.Title, dto.Title),
                ct);

            if (result == 0) { throw new NotFoundException(); }
        }
        catch (UniqueConstraintException) { throw new DuplicateException(nameof(dto.Title)); }

        var modification = new UserModification
        {
            UserId = GetCurrentUserId(),
            SubpageId = await dbContext.AboutUsSubpages.Where(p => p.Slug == dto.Slug).Select(p => p.SubpageId)
                    .FirstAsync(ct),
            HtmlContent = dto.HtmlContent
        };

        await dbContext.UserModifications.AddAsync(modification, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteSubpageAsync(string slug, CancellationToken ct = default)
    {
        await dbContext.AboutUsSubpages.Where(p => p.Slug == slug).ExecuteDeleteAsync(ct);
    }
}
