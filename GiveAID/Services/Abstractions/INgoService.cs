using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface INgoService
{
    Task<NgoSummaryDto[]> GetAllNgosAsync(CancellationToken ct = default);
    Task<NgoDto?> GetNgoByIdAsync(Guid id, CancellationToken ct = default);
    Task<NgoDto> CreateNgoAsync(NgoSaveDto dto, CancellationToken ct = default);
    Task UpdateNgoAsync(Guid id, NgoSaveDto dto, CancellationToken ct = default);
    Task DeleteNgoAsync(Guid id, CancellationToken ct = default);
}
