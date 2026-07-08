using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public interface IQueryService
{
    Task<List<T>> GetAllAsync<T>(CancellationToken ct = default) where T : class;
    Task<T?> GetByIdAsync<T>(int id, CancellationToken ct = default) where T : class;
    Task<List<T>> QueryAsync<T>(Func<IQueryable<T>, IQueryable<T>> query, CancellationToken ct = default) where T : class;
}

