using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IDonationCauseService
{
    Task<DonationCauseDto[]> GetAllDonationCausesAsync(CancellationToken ct = default);
    Task<DonationCauseDto?> GetDonationCauseByIdAsync(Guid id, CancellationToken ct = default);
    Task<DonationCauseDto> CreateDonationCauseAsync(DonationCauseSaveDto dto, CancellationToken ct = default);
    Task UpdateDonationCauseAsync(Guid id, DonationCauseSaveDto dto, CancellationToken ct = default);
    Task DeleteDonationCauseAsync(Guid id, CancellationToken ct = default);
}
