using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public interface IAboutService
{
    // View: lấy danh sách / lấy 1 subpage theo Id (dùng cho cả Preview)
    public Task<List<AboutSubpage>> GetAllSubpagesAsync(CancellationToken ct = default);
    public Task<AboutSubpage?> GetSubpageByIdAsync(Guid subpageId, CancellationToken ct = default);

    // Create
    public Task<AboutSubpage> CreateSubpageAsync(AboutSubpage subpage, CancellationToken ct = default);

    // Update
    public Task<AboutSubpage> UpdateSubpageAsync(AboutSubpage subpage, CancellationToken ct = default);

    // Delete
    public Task<bool> DeleteSubpageAsync(Guid subpageId, CancellationToken ct = default);
}