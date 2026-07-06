using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IDonationCauseService
{
    Task<DonationCauseDto[]> GetAllDonationCausesAsync(CancellationToken ct = default);
    Task<DonationCauseDto?> GetDonationCauseByIdAsync(Guid id, CancellationToken ct = default);
    Task<bool> CreateDonationCauseAsync(DonationCauseSaveDto dto, CancellationToken ct = default);
    Task<bool> UpdateDonationCauseAsync(Guid id, DonationCauseSaveDto dto, CancellationToken ct = default);
    Task<bool> DeleteDonationCauseAsync(Guid id, CancellationToken ct = default);
}
