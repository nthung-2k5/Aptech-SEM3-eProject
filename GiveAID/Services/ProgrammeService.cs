namespace GiveAID.Services;

using Abstractions;
using Dtos;
using System.Threading.Tasks;
using System;
using System.Linq;

public class ProgrammeService : IProgrammeService
{
    public Task<ProgrammeSummaryDto[]> GetProgrammesAsync(ProgrammeQueryParameters query, CancellationToken ct = default)
    {
        var q = Models.MockData.Programmes.AsQueryable();

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

        var paginated = q.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).Select(p =>
                new ProgrammeSummaryDto(
                    p.Id,
                    p.Name,
                    p.Cause,
                    p.Ngo,
                    p.ImageUrl,
                    p.StartDate,
                    p.EndDate,
                    p.DonationCount,
                    p.TargetAmount,
                    p.RaisedAmount)).ToArray();

        return Task.FromResult(paginated);
    }

    public Task<ProgrammeDetailsDto?> GetProgrammeDetailsAsync(Guid id, CancellationToken ct = default)
    {
        var prog = Models.MockData.Programmes.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(prog);
    }
}
