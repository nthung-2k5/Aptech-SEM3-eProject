using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IDonationService
{
    Task<DonationDto[]> GetAllDonationsAsync(CancellationToken ct = default);
    Task<UserDonationDto[]> GetUserDonationsAsync(Guid userId, CancellationToken ct = default);
    Task<DonationDto?> CreateDonationAsync(DonationSaveDto donation, CancellationToken ct = default);
    Task<bool> VoidDonationAsync(Guid donationId, CancellationToken ct = default);
}
