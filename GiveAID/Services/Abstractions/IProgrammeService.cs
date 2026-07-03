using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IProgrammeService
{
    Task<(IEnumerable<ProgrammeSummaryDto> Programmes, int TotalCount)> GetProgrammesAsync(ProgrammeQueryParameters query);
    Task<ProgrammeDetailsDto?> GetProgrammeDetailsAsync(Guid id);
}
