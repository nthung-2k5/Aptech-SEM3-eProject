using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IProgrammeService
{
    Task<ProgrammeSummaryDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query, CancellationToken ct = default);
    Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProgrammeDto?> CreateProgrammeAsync(ProgrammeSaveDto programme, CancellationToken ct = default);
    Task<bool> UpdateProgrammeAsync(Guid id, ProgrammeSaveDto programme, CancellationToken ct = default);
    Task<bool> DeleteProgrammeAsync(Guid id, CancellationToken ct = default);
}
