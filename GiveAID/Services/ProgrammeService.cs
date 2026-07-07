using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class ProgrammeService(AppDbContext dbContext) : IProgrammeService
{
    public async Task<ProgrammeSummaryDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query,
                                                                   CancellationToken ct = default)
    {
        var q = dbContext.WelfareProgrammes
            .AsNoTracking()
            .Where(p => !p.IsDeleted);

        if (query != null)
        {
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                q = q.Where(p => p.Name.Contains(query.SearchTerm) ||
                                 p.Description.Contains(query.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(query.Ngo))
            {
                q = q.Where(p => p.Ngo.Name == query.Ngo);
            }

            if (!string.IsNullOrWhiteSpace(query.Cause))
            {
                q = q.Where(p => p.Cause.Name == query.Cause);
            }
        }

        var pageNumber = query?.PageNumber ?? 1;
        var pageSize = query?.PageSize > 0 ? query.PageSize : 10;

        return await q
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProgrammeSummaryDto(
                p.ProgrammeId,
                p.Name,
                p.Cause.Name,
                p.Ngo.Name,
                p.ImageUrl,
                p.StartTime.DateTime,
                p.EndTime == null ? DateTime.MaxValue : p.EndTime.Value.DateTime,
                p.Donations.Count(d => d.Status == DonationStatus.Completed),
                p.MaxDonation,
                p.Donations.Where(d => d.Status == DonationStatus.Completed).Sum(d => d.Amount)
            ))
            .ToArrayAsync(ct);
    }

    public async Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.WelfareProgrammes
            .AsNoTracking()
            .Where(p => p.ProgrammeId == id && !p.IsDeleted)
            .Select(p => new ProgrammeDto(
                p.ProgrammeId,
                p.Name,
                p.Cause.Name,
                p.Ngo.Name,
                p.ImageUrl,
                p.StartTime.DateTime,
                p.EndTime == null ? DateTime.MaxValue : p.EndTime.Value.DateTime,
                p.Donations.Count(d => d.Status == DonationStatus.Completed),
                p.MaxDonation,
                p.Donations.Where(d => d.Status == DonationStatus.Completed).Sum(d => d.Amount),
                p.Description,
                p.Ngo.Name  // OrganizerInfo = the managing NGO's name
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ProgrammeDto?> CreateProgrammeAsync(ProgrammeSaveDto programme, CancellationToken ct = default)
    {
        // Validate FK: NGO must exist and not be deleted
        bool ngoExists = await dbContext.Ngos
            .AnyAsync(n => n.NgoId == programme.NgoId && !n.IsDeleted, ct);
        if (!ngoExists) return null;

        // Validate FK: DonationCause must exist and not be deleted
        bool causeExists = await dbContext.DonationCauses
            .AnyAsync(c => c.CauseId == programme.CauseId && !c.IsDeleted, ct);
        if (!causeExists) return null;

        var entity = new WelfareProgramme
        {
            NgoId = programme.NgoId,
            CauseId = programme.CauseId,
            Name = programme.Name,
            Description = programme.Description,
            ImageUrl = programme.ImageUrl,
            StartTime = programme.StartTime,
            EndTime = programme.EndTime,
            MaxDonation = programme.MaxDonation,
            Location = programme.Location,
        };

        dbContext.WelfareProgrammes.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        // Re-query to get the full DTO with navigated Ngo.Name / Cause.Name
        return await GetProgrammeByIdAsync(entity.ProgrammeId, ct);
    }

    public async Task<bool> UpdateProgrammeAsync(Guid id, ProgrammeSaveDto programme, CancellationToken ct = default)
    {
        // Validate FK: NGO must exist and not be deleted
        bool ngoExists = await dbContext.Ngos
            .AnyAsync(n => n.NgoId == programme.NgoId && !n.IsDeleted, ct);
        if (!ngoExists) return false;

        // Validate FK: DonationCause must exist and not be deleted
        bool causeExists = await dbContext.DonationCauses
            .AnyAsync(c => c.CauseId == programme.CauseId && !c.IsDeleted, ct);
        if (!causeExists) return false;

        var entity = await dbContext.WelfareProgrammes
            .FirstOrDefaultAsync(p => p.ProgrammeId == id && !p.IsDeleted, ct);

        if (entity == null) return false;

        entity.NgoId = programme.NgoId;
        entity.CauseId = programme.CauseId;
        entity.Name = programme.Name;
        entity.Description = programme.Description;
        entity.ImageUrl = programme.ImageUrl;
        entity.StartTime = programme.StartTime;
        entity.EndTime = programme.EndTime;
        entity.MaxDonation = programme.MaxDonation;
        entity.Location = programme.Location;

        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteProgrammeAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.WelfareProgrammes
            .Where(p => p.ProgrammeId == id && !p.IsDeleted)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true), ct) > 0;
    }
}
