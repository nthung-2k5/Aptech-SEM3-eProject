using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class UserQueryService(AppDbContext dbContext) : IUserQueryService
{
    public async Task<UserQueryDto[]> GetAllQueriesAsync(CancellationToken ct = default)
    {
        return await dbContext.UserQueries
            .AsNoTracking()
            .Include(q => q.User)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new UserQueryDto(q.QueryId, q.UserId, q.User.FullName, q.Subject, q.MessageText, q.ReplyText, q.CreatedAt))
            .ToArrayAsync(ct);
    }

    public async Task<UserQueryDto[]> GetUnansweredQueriesAsync(CancellationToken ct = default)
    {
        return await dbContext.UserQueries
            .AsNoTracking()
            .Include(q => q.User)
            .Where(q => q.ReplyText == null)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new UserQueryDto(q.QueryId, q.UserId, q.User.FullName, q.Subject, q.MessageText, q.ReplyText, q.CreatedAt))
            .ToArrayAsync(ct);
    }

    public Task<bool> CreateQueryAsync(UserQueryCreateDto query, CancellationToken ct = default)
        => throw new NotImplementedException();

    public async Task<bool> ReplyQueryAsync(Guid id, string replyText, CancellationToken ct = default)
    {
        int rows = await dbContext.UserQueries
            .Where(q => q.QueryId == id && q.ReplyText == null)
            .ExecuteUpdateAsync(s => s.SetProperty(q => q.ReplyText, replyText), ct);
        return rows > 0;
    }
}
