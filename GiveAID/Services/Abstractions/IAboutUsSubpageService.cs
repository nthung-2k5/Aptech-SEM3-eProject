using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IAboutUsSubpageService
{
    Task<AboutUsSubpageDto[]> ListSubpagesAsync(CancellationToken ct = default);
    AboutUsSubpageDto? GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<bool> AddSubpageAsync(AboutUsSubpageDto page, CancellationToken ct = default);
    Task<bool> UpdateSubpageAsync(AboutUsSubpageDto page, CancellationToken ct = default);
    Task<bool> DeleteSubpageAsync(string slug, CancellationToken ct = default);
}
