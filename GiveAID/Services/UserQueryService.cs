using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;

namespace GiveAID.Services;

public class UserQueryService : IUserQueryService
{
    public Task<UserQueryDto[]> GetAllQueriesAsync(CancellationToken ct = default) =>
            Task.FromResult(MockData.Queries);

    public Task<UserQueryDto[]> GetUnansweredQueriesAsync(CancellationToken ct = default) =>
            Task.FromResult(MockData.Queries.Where(q => string.IsNullOrEmpty(q.ReplyText)).ToArray());

    public Task<bool> CreateQueryAsync(UserQueryCreateDto query, CancellationToken ct = default) =>
            throw new NotImplementedException();

    public Task<bool> ReplyQueryAsync(Guid id, string replyText, CancellationToken ct = default) =>
            throw new NotImplementedException();
}
