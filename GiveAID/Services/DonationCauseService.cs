using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class DonationCauseService(AppDbContext dbContext): IDonationCauseService
{
    public async Task<DonationCauseDto[]> GetAllDonationCausesAsync(CancellationToken ct = default)
    {
        return await dbContext.ActiveDonationCauses
                .AsNoTracking()
                .ProjectToDto()
                .ToArrayAsync(ct);
    }

    public async Task<DonationCauseDto?> GetDonationCauseByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.ActiveDonationCauses
                .AsNoTracking()
                .Where(cause => cause.CauseId == id)
                .ProjectToDto()
                .FirstOrDefaultAsync(ct);
    }

    public async Task<bool> CreateDonationCauseAsync(DonationCauseSaveDto dto, CancellationToken ct = default)
    {
        bool exists = await dbContext.ActiveDonationCauses.AnyAsync(cause => cause.Name == dto.Name, ct);
        
        if (exists) return false;
        
        await dbContext.DonationCauses.AddAsync(dto.ToEntity(), ct);
        await dbContext.SaveChangesAsync(ct);
        
        return true;
    }

    public async Task<bool> UpdateDonationCauseAsync(Guid id, DonationCauseSaveDto dto,
                                               CancellationToken ct = default)
    {
        var donationCause = await dbContext.DonationCauses.FindAsync([id], ct);
        if (donationCause == null || donationCause.IsDeleted)
        {
            return false;
        }
        
        bool duplicateNameExists = await dbContext.ActiveDonationCauses
                .AnyAsync(cause => cause.Name == dto.Name && cause.CauseId != id, ct);

        if (duplicateNameExists)
        {
            throw new DuplicateNameException($"A donation cause with the name '{dto.Name}' already exists.");
        }
        
        donationCause.Name = dto.Name;
        await dbContext.SaveChangesAsync(ct);
        
        return true;
    }

    public async Task<bool> DeleteDonationCauseAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.ActiveDonationCauses
                .Where(cause => cause.CauseId == id)
                .ExecuteUpdateAsync(cause => cause.SetProperty(c => c.IsDeleted, true), ct) > 0;
    }
}

public class DuplicateNameException(string message) : Exception(message);
