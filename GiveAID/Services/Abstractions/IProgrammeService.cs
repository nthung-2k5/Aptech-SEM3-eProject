using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IProgrammeService
{
    Task<PagedResult<ProgrammeDto>> GetAllProgrammesPagedAsync(ProgrammeQueryParameters query, CancellationToken ct = default);
    Task<ProgrammeDto[]> GetAvailableProgrammesAsync(ProgrammeQueryParameters? query, CancellationToken ct = default);
    Task<ProgrammeDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query, CancellationToken ct = default);
    Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProgrammeSaveDto?> GetProgrammeSaveDtoByIdAsync(Guid id, CancellationToken ct = default);
    Task<ProgrammeDto> CreateProgrammeAsync(ProgrammeSaveDto dto, CancellationToken ct = default);
    Task UpdateProgrammeAsync(Guid id, ProgrammeSaveDto dto, CancellationToken ct = default);
    Task DeleteProgrammeAsync(Guid id, CancellationToken ct = default);
}
