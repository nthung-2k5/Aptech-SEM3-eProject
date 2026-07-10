using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IPartnerService
{
    Task<PagedResult<PartnerDto>> GetPartnersPagedAsync(PartnerQueryParameters query, CancellationToken ct = default);
    Task<PartnerSummaryDto[]> GetAllPartnersAsync(CancellationToken ct = default);
    Task<PartnerDto[]> GetAllPartnerDtosAsync(CancellationToken ct = default);
    Task<PartnerDto?> GetPartnerByIdAsync(Guid id, CancellationToken ct = default);
    Task<PartnerDto> CreatePartnerAsync(PartnerSaveDto dto, CancellationToken ct = default);
    Task UpdatePartnerAsync(Guid id, PartnerSaveDto dto, CancellationToken ct = default);
    Task DeletePartnerAsync(Guid id, CancellationToken ct = default);
}
