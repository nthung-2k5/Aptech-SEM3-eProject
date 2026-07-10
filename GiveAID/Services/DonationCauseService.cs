using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class DonationCauseService(AppDbContext dbContext) : IDonationCauseService
{
    public async Task<DonationCauseDto[]> GetAllDonationCausesAsync(CancellationToken ct = default) =>
            await dbContext.ActiveDonationCauses.AsNoTracking().ProjectToDto().ToArrayAsync(ct);

    public async Task<DonationCauseDto?> GetDonationCauseByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.ActiveDonationCauses.AsNoTracking().Where(cause => cause.CauseId == id).ProjectToDto()
                .FirstOrDefaultAsync(ct);
    }

    public async Task<DonationCauseDto> CreateDonationCauseAsync(DonationCauseSaveDto dto,
                                                                 CancellationToken ct = default)
    {
        bool exists = await dbContext.ActiveDonationCauses.AnyAsync(cause => cause.Name == dto.Name, ct);

        if (exists) { throw new DuplicateException(nameof(dto.Name)); }

        var entity = dto.ToEntity();

        await dbContext.DonationCauses.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);

        return entity.ToDto();
    }

    public async Task UpdateDonationCauseAsync(Guid id, DonationCauseSaveDto dto, CancellationToken ct = default)
    {
        var donationCause = await dbContext.DonationCauses.FindAsync([id], ct);

        if (donationCause == null || donationCause.IsDeleted) { throw new NotFoundException(); }

        bool duplicateNameExists = await dbContext.ActiveDonationCauses.AnyAsync(
            cause => cause.Name == dto.Name && cause.CauseId != id,
            ct);

        if (duplicateNameExists) { throw new DuplicateException(nameof(dto.Name)); }

        donationCause.Name = dto.Name;
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteDonationCauseAsync(Guid id, CancellationToken ct = default)
    {
        await dbContext.ActiveDonationCauses.Where(cause => cause.CauseId == id).ExecuteUpdateAsync(
            cause => cause.SetProperty(c => c.IsDeleted, true),
            ct);
    }
}
