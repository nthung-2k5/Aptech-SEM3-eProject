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
    private readonly List<AboutUsSubpageDto> pages =
    [
        new(
            "what-we-do",
            "What We Do",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">What We Do</h2>
                    <p class="text-lg text-[#6B6459] leading-relaxed">Give-AID coordinates fundraising and welfare programme delivery across a network of verified NGO partners.</p>
                    <div class="grid md:grid-cols-2 gap-6">
                        <div class="bg-[#F6F1E9] rounded-xl p-6">
                            <h4 class="font-['Playfair_Display',serif] text-lg font-bold text-[#1C1A17] mb-2">Fund Mobilisation</h4>
                            <p class="text-sm text-[#6B6459]">We channel corporate and individual donations to high-impact causes with full transparency.</p>
                        </div>
                        <div class="bg-[#F6F1E9] rounded-xl p-6">
                            <h4 class="font-['Playfair_Display',serif] text-lg font-bold text-[#1C1A17] mb-2">Programme Management</h4>
                            <p class="text-sm text-[#6B6459]">End-to-end coordination of health, education, and social inclusion programmes.</p>
                        </div>
                    </div>
                </div>
                """),

        new(
            "our-mission",
            "Our Mission",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">Our Mission</h2>
                    <div class="border-l-4 border-[#C97C2E] pl-6 py-2">
                        <p class="font-['Playfair_Display',serif] text-2xl italic text-[#1A5C6B]">"To build an equitable world where every person — regardless of circumstance — has access to healthcare, education, and dignity."</p>
                    </div>
                    <p class="text-[#6B6459] leading-relaxed">Our mission drives every decision we make: from which NGOs we partner with, to how donations are allocated, to the events we sponsor and the communities we serve.</p>
                </div>
                """),

        new(
            "our-team",
            "Our Team",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">Our Team</h2>
                    <p>Our team comprises dedicated professionals.</p>
                </div>
                """),

        new(
            "career",
            "Career With Us",
            """
                <div class="space-y-6">
                    <h2 class="font-['Playfair_Display',serif] text-4xl font-bold text-[#1C1A17]">Career With Us</h2>
                    <p>Join a passionate team working to change the world.</p>
                </div>
                """)
    ];

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
