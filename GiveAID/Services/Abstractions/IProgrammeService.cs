using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IProgrammeService
{
    Task<ProgrammeSummaryDto[]> GetProgrammesAsync(ProgrammeQueryParameters query, CancellationToken ct = default);

    Task<ProgrammeDetailsDto?> GetProgrammeDetailsAsync(Guid id, CancellationToken ct = default);
}
