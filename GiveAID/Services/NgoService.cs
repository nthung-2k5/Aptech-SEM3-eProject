using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class NgoService : INgoService
{
    public Task<NgoSummaryDto[]> GetAllNgosAsync(CancellationToken ct = default) =>
            Task.FromResult<NgoSummaryDto[]>(MockData.Ngos);

    public Task<NgoDto?> GetNgoByIdAsync(Guid id, CancellationToken ct = default) =>
        Task.FromResult(MockData.Ngos.FirstOrDefault(m => m.Id == id));

    public Task<NgoDto?> CreateNgoAsync(NgoSaveDto ngo, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task<bool> UpdateNgoAsync(Guid id, NgoSaveDto ngo, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task<bool> DeleteNgoAsync(Guid id, CancellationToken ct = default) => throw new NotImplementedException();
}
