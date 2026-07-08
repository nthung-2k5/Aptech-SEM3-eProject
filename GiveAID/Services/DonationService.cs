using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class DonationService(AppDbContext dbContext) : IDonationService
{
    public async Task<DonationDto[]> GetAllDonationsAsync(CancellationToken ct = default)
    {
        var donations = dbContext.Donations.Include(d => d.User).Include(d => d.Ngo).Include(d => d.Cause)
                .Include(d => d.Programme);

        return await donations.ProjectToDto().ToArrayAsync(ct);
    }

    public async Task<UserDonationDto[]> GetDonationsByUserAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.ValidDonations.Include(d => d.Ngo).Include(d => d.Cause).Include(d => d.Programme)
                .Where(d => d.UserId == userId).ProjectToUserDto().ToArrayAsync(ct);
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

        return entity.ToDto();
    }

    public async Task VoidDonationAsync(Guid donationId, CancellationToken ct = default)
    {
        await dbContext.ValidDonations.Where(d => d.DonationId == donationId).ExecuteUpdateAsync(
            s => s.SetProperty(d => d.Status, DonationStatus.Void),
            ct);
    }
}
