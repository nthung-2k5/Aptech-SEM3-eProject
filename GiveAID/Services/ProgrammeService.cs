using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class ProgrammeService(AppDbContext context, IUserInterestService userInterestService) : IProgrammeService
{
    public async Task<PagedResult<ProgrammeDto>> GetAllProgrammesPagedAsync(ProgrammeQueryParameters query,
                                                                            CancellationToken ct = default)
    {
        IQueryable<WelfareProgramme> q = context.ActiveWelfareProgrammes.AsNoTracking().Include(p => p.Ngo)
                .Include(p => p.Cause).Include(p => p.Donations);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            q = q.Where(p => p.Name.Contains(query.SearchTerm) || p.Description.Contains(query.SearchTerm));
        }

        if (query.NgoId != null) { q = q.Where(p => p.Ngo.NgoId == query.NgoId); }

        if (query.CauseId != null) { q = q.Where(p => p.Cause.CauseId == query.CauseId); }

        if (!string.IsNullOrWhiteSpace(query.StatusFilter))
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            q = query.StatusFilter switch
            {
                "Active" => q.Where(p => p.StartDate <= now && (!p.EndDate.HasValue || p.EndDate > now)),
                "Upcoming" => q.Where(p => p.StartDate > now),
                "Ended" => q.Where(p => p.EndDate.HasValue && p.EndDate < now),
                _ => q
            };
        }

        var totalCount = await q.CountAsync(ct);

        q = query.SortBy?.ToLower() switch
        {
            "name" => query.SortDescending ? q.OrderByDescending(p => p.Name) : q.OrderBy(p => p.Name),
            "target" => query.SortDescending ? q.OrderByDescending(p => p.MaxDonation) : q.OrderBy(p => p.MaxDonation),
            "startdate" => query.SortDescending ? q.OrderByDescending(p => p.StartDate) : q.OrderBy(p => p.StartDate),
            "enddate" => query.SortDescending ? q.OrderByDescending(p => p.EndDate) : q.OrderBy(p => p.EndDate),
            _ => query.SortDescending ? q.OrderByDescending(p => p.StartDate) : q.OrderBy(p => p.StartDate)
        };

        var items = await q.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .ProjectToDto().ToArrayAsync(ct);

        return new PagedResult<ProgrammeDto>(items, totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<ProgrammeDto[]> GetAvailableProgrammesAsync(ProgrammeQueryParameters? query,
                                                                  CancellationToken ct = default)
    {
        IQueryable<WelfareProgramme> q = context.AvailableWelfareProgrammes.AsNoTracking().Include(p => p.Ngo)
                .Include(p => p.Cause).Include(p => p.Donations);

        q = ApplyQueryParameters(q, query);

        return await q.ProjectToDto().ToArrayAsync(ct);
    }

    public async Task<ProgrammeDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query,
                                                            CancellationToken ct = default)
    {
        IQueryable<WelfareProgramme> q = context.ActiveWelfareProgrammes.AsNoTracking().Include(p => p.Ngo)
                .Include(p => p.Cause).Include(p => p.Donations);

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

        if (parameters.NgoId != null) { query = query.Where(p => p.Ngo.NgoId == parameters.NgoId); }

        if (parameters.CauseId != null) { query = query.Where(p => p.Cause.CauseId == parameters.CauseId); }

        query = parameters.SortBy?.ToLower() switch
        {
            "name" => parameters.SortDescending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "target" => parameters.SortDescending ? query.OrderByDescending(p => p.MaxDonation) : query.OrderBy(p => p.MaxDonation),
            "startdate" => parameters.SortDescending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate),
            "enddate" => parameters.SortDescending ? query.OrderByDescending(p => p.EndDate) : query.OrderBy(p => p.EndDate),
            _ => parameters.SortDescending ? query.OrderByDescending(p => p.StartDate) : query.OrderBy(p => p.StartDate)
        };

        return query.Skip((parameters.PageNumber - 1) * parameters.PageSize).Take(parameters.PageSize);
    }

    public async Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.ActiveWelfareProgrammes.AsNoTracking().Include(p => p.Ngo).Include(p => p.Cause)
                .Include(p => p.Donations).Where(p => p.ProgrammeId == id).ProjectToDto().FirstOrDefaultAsync(ct);
    }

    public async Task<ProgrammeSaveDto?> GetProgrammeSaveDtoByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.ActiveWelfareProgrammes.AsNoTracking()
            .Where(p => p.ProgrammeId == id)
            .Select(p => new ProgrammeSaveDto(p.NgoId, p.CauseId, p.Name, p.ImageUrl, p.Description, p.StartDate, p.EndDate, p.MaxDonation, p.Location))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<ProgrammeDto> CreateProgrammeAsync(ProgrammeSaveDto dto, CancellationToken ct = default)
    {
        // Validate FK: NGO must exist and not be deleted
        if (!await context.ActiveNgos.AnyAsync(n => n.NgoId == dto.NgoId, ct))
        {
            throw new MissingForeignEntityException(nameof(dto.NgoId));
        }

        // Validate FK: DonationCause must exist and not be deleted
        if (!await context.ActiveDonationCauses.AnyAsync(c => c.CauseId == dto.CauseId, ct))
        {
            throw new MissingForeignEntityException(nameof(dto.CauseId));
        }

        var programme = dto.ToEntity();

        context.WelfareProgrammes.Add(programme);
        await context.SaveChangesAsync(ct);

        await userInterestService.NotifyInterestedUsersAsync(programme, ct);

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
        if (!await context.ActiveDonationCauses.AnyAsync(c => c.CauseId == dto.CauseId, ct))
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
        programme.StartDate = dto.StartDate;
        programme.EndDate = dto.EndDate;
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
