using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IProgrammeService
{
    Task<ProgrammeSummaryDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query, CancellationToken ct = default);
    Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default);
}
