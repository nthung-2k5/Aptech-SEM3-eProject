using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class DonationService(AppDbContext dbContext, INotificationService notificationService) : IDonationService
{
    public async Task<PagedResult<DonationDto>> GetDonationsPagedAsync(DonationQueryParameters query, CancellationToken ct = default)
    {
        var q = dbContext.Donations.Include(d => d.User).Include(d => d.Ngo).Include(d => d.Cause)
                .Include(d => d.Programme).AsQueryable();

        if (query.DateFrom.HasValue)
        {
            var from = query.DateFrom.Value.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
            q = q.Where(d => d.CreatedAt >= from);
        }

        if (query.DateTo.HasValue)
        {
            // inclusive: end of the selected day
            var to = query.DateTo.Value.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
            q = q.Where(d => d.CreatedAt <= to);
        }

        if (query.Status.HasValue)
        {
            q = q.Where(d => d.Status == query.Status.Value);
        }

        if (query.ProgrammeId.HasValue)
        {
            q = q.Where(d => d.ProgrammeId == query.ProgrammeId.Value);
        }

        if (query.NgoId.HasValue)
        {
            q = q.Where(d => d.NgoId == query.NgoId.Value);
        }

        if (query.CauseId.HasValue)
        {
            q = q.Where(d => d.CauseId == query.CauseId.Value);
        }

        var totalCount = await q.CountAsync(ct);
        var items = await q.OrderByDescending(d => d.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .ProjectToDto().ToArrayAsync(ct);

        return new PagedResult<DonationDto>(items, totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<DonationDto[]> GetAllDonationsAsync(CancellationToken ct = default)
    {
        var donations = dbContext.Donations.Include(d => d.User).Include(d => d.Ngo).Include(d => d.Cause)
                .Include(d => d.Programme);

        return await donations.ProjectToDto().ToArrayAsync(ct);
    }

    public async Task<PagedResult<UserDonationDto>> GetDonationsByUserPagedAsync(Guid userId, UserDonationQueryParameters query, CancellationToken ct = default)
    {
        var q = dbContext.ValidDonations.Include(d => d.Ngo).Include(d => d.Cause).Include(d => d.Programme)
                .Where(d => d.UserId == userId);

        if (query.TimePeriod == "30days")
        {
            var from = DateTime.UtcNow.AddDays(-30);
            q = q.Where(d => d.CreatedAt >= from);
        }
        else if (query.TimePeriod == "6months")
        {
            var from = DateTime.UtcNow.AddMonths(-6);
            q = q.Where(d => d.CreatedAt >= from);
        }
        else if (query.TimePeriod == "year")
        {
            var from = new DateTime(DateTime.UtcNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            q = q.Where(d => d.CreatedAt >= from);
        }

        if (query.MinAmount.HasValue)
        {
            q = q.Where(d => d.Amount >= query.MinAmount.Value);
        }

        if (query.MaxAmount.HasValue)
        {
            q = q.Where(d => d.Amount <= query.MaxAmount.Value);
        }

        if (query.TargetNgo && !query.TargetProgramme)
        {
            q = q.Where(d => d.NgoId != null);
        }
        else if (query.TargetProgramme && !query.TargetNgo)
        {
            q = q.Where(d => d.ProgrammeId != null);
        }

        var totalCount = await q.CountAsync(ct);
        var items = await q.OrderByDescending(d => d.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize)
                .ProjectToUserDto()
                .ToArrayAsync(ct);

        return new PagedResult<UserDonationDto>(items, totalCount, query.PageNumber, query.PageSize);
    }

    public async Task<DonationDto?> CreateDonationAsync(DonationSaveDto donation, CancellationToken ct = default)
    {
        if (donation.Target.TryPickT0(out var ngo, out var programme))
        {
            bool ngoExists = await dbContext.ActiveNgos.AnyAsync(n => n.NgoId == ngo.NgoId, ct);
            bool causeExists = await dbContext.ActiveDonationCauses.AnyAsync(c => c.CauseId == ngo.CauseId, ct);

            if (!ngoExists || !causeExists) { throw new MissingForeignEntityException(nameof(donation.Target)); }
        }
        else
        {
            if (!await dbContext.ActiveWelfareProgrammes.AnyAsync(p => p.ProgrammeId == programme.ProgrammeId, ct))
            {
                throw new MissingForeignEntityException(nameof(donation.Target));
            }
        }
        
        var entity = donation.ToEntity();

        await dbContext.Donations.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
        
        await notificationService.CreateNotificationAsync(donation.UserId, $"Your donation of ${donation.Amount:N0} was successfully received. Thank you for your generosity!", ct);

        return await dbContext.Donations
                .Include(d => d.Ngo)
                .Include(d => d.Cause)
                .Include(d => d.Programme)
                .Where(d => d.DonationId == entity.DonationId)
                .ProjectToDto().FirstOrDefaultAsync(ct);
    }

    public async Task VoidDonationAsync(Guid donationId, CancellationToken ct = default)
    {
        await dbContext.ValidDonations.Where(d => d.DonationId == donationId).ExecuteUpdateAsync(
            s => s.SetProperty(d => d.Status, DonationStatus.Void),
            ct);
    }
}
