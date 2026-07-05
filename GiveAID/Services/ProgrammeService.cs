using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class ProgrammeService : IProgrammeService
{
    public Task<ProgrammeSummaryDto[]> GetAllProgrammesAsync(ProgrammeQueryParameters? query,
                                                          CancellationToken ct = default)
    {
        var q = MockData.Programmes.AsQueryable();

        if (query != null)
        {
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                q = q.Where(p => p.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                 p.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Ngo))
            {
                q = q.Where(p => p.Ngo.Equals(query.Ngo, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.Cause))
            {
                q = q.Where(p => p.Cause.Equals(query.Cause, StringComparison.OrdinalIgnoreCase));
            }

            q = q.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize);
        }

        return Task.FromResult<ProgrammeSummaryDto[]>(q.ToArray());
    }

    public Task<ProgrammeDto?> GetProgrammeByIdAsync(Guid id, CancellationToken ct = default)
    {
        var prog = MockData.Programmes.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(prog);
    }
}
