using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IAboutUsSubpageService
{
    Task<AboutUsSubpageSummaryDto[]> ListSubpagesAsync(CancellationToken ct = default);
    Task<AboutUsSubpageDto?> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task AddSubpageAsync(AboutUsSubpageDto dto, CancellationToken ct = default);
    Task UpdateSubpageAsync(AboutUsSubpageDto dto, CancellationToken ct = default);
    Task DeleteSubpageAsync(string slug, CancellationToken ct = default);
}
