using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface INgoService
{
    Task<NgoSummaryDto[]> GetAllNgosAsync(CancellationToken ct = default);
    Task<NgoDto?> GetNgoByIdAsync(Guid id, CancellationToken ct = default);
    Task<NgoDto?> CreateNgoAsync(NgoSaveDto ngo, CancellationToken ct = default);
    Task<bool> UpdateNgoAsync(Guid id, NgoSaveDto ngo, CancellationToken ct = default);
    Task<bool> DeleteNgoAsync(Guid id, CancellationToken ct = default);
}
