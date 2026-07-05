using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IAboutUsSubpageService
{
    Task<AboutUsSubpageSummaryDto[]> ListSubpagesAsync(CancellationToken ct = default);
    Task<AboutUsSubpageDetailsDto?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<bool> AddSubpageAsync(AboutUsSubpageDetailsDto page, CancellationToken ct = default);
    Task<bool> UpdateSubpageAsync(AboutUsSubpageDetailsDto page, CancellationToken ct = default);
    Task<bool> DeleteSubpageAsync(string slug, CancellationToken ct = default);
}
