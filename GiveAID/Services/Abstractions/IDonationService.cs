using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IDonationService
{
    Task<PagedResult<DonationDto>> GetDonationsPagedAsync(DonationQueryParameters query, CancellationToken ct = default);
    Task<DonationDto[]> GetAllDonationsAsync(CancellationToken ct = default);
    Task<PagedResult<UserDonationDto>> GetDonationsByUserPagedAsync(Guid userId, UserDonationQueryParameters query, CancellationToken ct = default);
    Task<DonationDto?> CreateDonationAsync(DonationSaveDto donation, CancellationToken ct = default);
    Task VoidDonationAsync(Guid donationId, CancellationToken ct = default);
}
