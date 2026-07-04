using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface IUserQueryService
{
    Task<List<UserQueryDto>> GetAllQueriesAsync(CancellationToken ct = default);
    Task<List<UserQueryDto>> GetUnansweredQueriesAsync(CancellationToken ct = default);
    Task<bool> CreateQueryAsync(UserQueryCreateDto query, CancellationToken ct = default);
    Task<bool> ReplyQueryAsync(Guid id, string replyText, CancellationToken ct = default);
}
