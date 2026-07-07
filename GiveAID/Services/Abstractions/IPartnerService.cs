using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IPartnerService
{
    Task<PartnerSummaryDto[]> GetAllPartnersAsync(CancellationToken ct = default);
    Task<PartnerDto?> GetPartnerByIdAsync(Guid id, CancellationToken ct = default);
    Task<PartnerDto?> CreatePartnerAsync(PartnerSaveDto partner, CancellationToken ct = default);
    Task<bool> UpdatePartnerAsync(Guid id, PartnerSaveDto partner, CancellationToken ct = default);
    Task<bool> DeletePartnerAsync(Guid id, CancellationToken ct = default);
}