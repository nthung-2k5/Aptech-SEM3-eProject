using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class DonationService : IDonationService
{
    public Task<DonationDto[]> GetAllDonationsAsync(CancellationToken ct = default) =>
            Task.FromResult(MockData.Donations);

    public Task<UserDonationDto[]> GetDonationsByUserAsync(Guid userId, CancellationToken ct = default)
    {
        return Task.FromResult(
            MockData.Donations.Select(d => new UserDonationDto(d.Target, d.Amount, d.DonationDate)).ToArray());
    }

    public Task<DonationDto?> CreateDonationAsync(DonationSaveDto donation, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task<bool> VoidDonationAsync(Guid donationId, CancellationToken ct = default) =>
            throw new NotImplementedException();
}
