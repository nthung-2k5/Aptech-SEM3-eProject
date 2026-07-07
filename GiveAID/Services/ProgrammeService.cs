using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class ProgrammeService(AppDbContext context, IUserInterestService userInterestService) : IProgrammeService
{
    public async Task<ProgrammeDto[]> GetAvailableProgrammesAsync(ProgrammeQueryParameters? query,
                                                                     CancellationToken ct = default)
    {
        var q = context.AvailableWelfareProgrammes.Include(p => p.Ngo).Include(p => p.Cause).Include(p => p.Donations)
                .AsQueryable();

        q = ApplyQueryParameters(q, query);

        return await q.ProjectToDto().ToArrayAsync(ct);
    }

    public async Task<ProgrammeDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query,
                                                            CancellationToken ct = default)
    {
        IQueryable<WelfareProgramme> q = context.AvailableWelfareProgrammes.Include(p => p.Ngo).Include(p => p.Cause)
                .Include(p => p.Donations);

        q = ApplyQueryParameters(q, query);

        return await q.ProjectToDto().ToArrayAsync(ct);
    }

    private static IQueryable<WelfareProgramme> ApplyQueryParameters(IQueryable<WelfareProgramme> query,
                                                                     ProgrammeQueryParameters? parameters)
    {
        if (parameters == null) { return query; }

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            query = query.Where(p => p.Name.Contains(parameters.SearchTerm) ||
                                     p.Description.Contains(parameters.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Ngo))
        {
            query = query.Where(p => p.Ngo.Name.Contains(parameters.Ngo));
        }

        if (!string.IsNullOrWhiteSpace(parameters.Cause))
        {
            query = query.Where(p => p.Cause.Name.Contains(parameters.Cause));
        }

        return query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
    }

    public async Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.ActiveWelfareProgrammes.Include(p => p.Ngo).Include(p => p.Cause)
                .Include(p => p.Donations).ProjectToDto().FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<ProgrammeDto> CreateProgrammeAsync(ProgrammeSaveDto dto, CancellationToken ct = default)
    {
        // Validate FK: NGO must exist and not be deleted
        if (!await context.ActiveNgos.AnyAsync(n => n.NgoId == dto.NgoId, ct))
        {
            throw new MissingForeignEntityException(nameof(dto.NgoId));
        }

        // Validate FK: DonationCause must exist and not be deleted
        if (await context.ActiveDonationCauses.AnyAsync(c => c.CauseId == dto.CauseId, ct))
        {
            throw new MissingForeignEntityException(nameof(dto.CauseId));
        }

        var programme = dto.ToEntity();

        context.WelfareProgrammes.Add(programme);
        await context.SaveChangesAsync(ct);

        var ngo = await context.Ngos.FindAsync([dto.NgoId], ct);

        if (ngo != null)
        {
            await userInterestService.NotifyInterestedUsersAsync(
                dto.NgoId,
                $"New Programme launched by {ngo.Name}: {programme.Name}");
        }

        return programme.ToDto();
    }

    public async Task UpdateProgrammeAsync(Guid id, ProgrammeSaveDto dto, CancellationToken ct = default)
    {
        // Validate FK: NGO must exist and not be deleted
        if (!await context.ActiveNgos.AnyAsync(n => n.NgoId == dto.NgoId, ct))
        {
            throw new MissingForeignEntityException(nameof(dto.NgoId));
        }

        // Validate FK: DonationCause must exist and not be deleted
        if (await context.ActiveDonationCauses.AnyAsync(c => c.CauseId == dto.CauseId, ct))
        {
            throw new MissingForeignEntityException(nameof(dto.CauseId));
        }

        var programme = await context.WelfareProgrammes.FindAsync([id], ct);
        
        if (programme == null || programme.IsDeleted) { throw new NotFoundException(); }

        programme.NgoId = dto.NgoId;
        programme.CauseId = dto.CauseId;
        programme.Name = dto.Name;
        programme.ImageUrl = dto.ImageUrl;
        programme.Description = dto.Description;
        programme.StartTime = dto.StartTime;
        programme.EndTime = dto.EndTime;
        programme.MaxDonation = dto.MaxDonation;
        programme.Location = dto.Location;

        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteProgrammeAsync(Guid id, CancellationToken ct = default)
    {
        await context.ActiveWelfareProgrammes.Where(p => p.ProgrammeId == id)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.IsDeleted, true), ct);
    }
}
