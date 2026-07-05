using GiveAID.Data;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class AboutService(AppDbContext dbContext) : IAboutService
{
    public async Task<List<AboutSubpage>> GetAllSubpagesAsync(CancellationToken ct = default)
    {
        return await dbContext.AboutSubpages.ToListAsync(ct);
    }

    public async Task<AboutSubpage?> GetSubpageByIdAsync(Guid subpageId, CancellationToken ct = default)
    {
        return await dbContext.AboutSubpages
            .FirstOrDefaultAsync(s => s.SubpageId == subpageId, ct);
    }

    public async Task<AboutSubpage> CreateSubpageAsync(AboutSubpage subpage, CancellationToken ct = default)
    {
        dbContext.AboutSubpages.Add(subpage);
        await dbContext.SaveChangesAsync(ct);
        return subpage;
    }

    public async Task<AboutSubpage> UpdateSubpageAsync(AboutSubpage subpage, CancellationToken ct = default)
    {
        subpage.UpdatedAt = DateTime.UtcNow;
        dbContext.AboutSubpages.Update(subpage);
        await dbContext.SaveChangesAsync(ct);
        return subpage;
    }

    public async Task<bool> DeleteSubpageAsync(Guid subpageId, CancellationToken ct = default)
    {
        var subpage = await dbContext.AboutSubpages
            .FirstOrDefaultAsync(s => s.SubpageId == subpageId, ct);

        if (subpage != null)
        {
            dbContext.AboutSubpages.Remove(subpage);
            await dbContext.SaveChangesAsync(ct);
            return true;
        }

        return false;
    }
}