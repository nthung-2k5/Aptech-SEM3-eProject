using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class UserInterestService(AppDbContext dbContext, INotificationService notificationService)
        : IUserInterestService
{
    public async Task FollowNgoAsync(Guid userId, Guid ngoId, CancellationToken ct = default)
    {
        if (!await dbContext.UserInterests.AnyAsync(ui => ui.UserId == userId && ui.NgoId == ngoId, ct))
        {
            dbContext.UserInterests.Add(new UserInterest { UserId = userId, NgoId = ngoId });
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public async Task UnfollowNgoAsync(Guid userId, Guid ngoId, CancellationToken ct = default)
    {
        await dbContext.UserInterests.Where(ui => ui.UserId == userId && ui.NgoId == ngoId).ExecuteDeleteAsync(ct);
    }

    public async Task<bool> IsFollowingNgoAsync(Guid userId, Guid ngoId, CancellationToken ct = default)
    {
        return await dbContext.UserInterests.AnyAsync(ui => ui.UserId == userId && ui.NgoId == ngoId, ct);
    }

    public async Task<NgoSummaryDto[]> GetUserInterestsAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.UserInterests.AsNoTracking().Where(ui => ui.UserId == userId).Include(ui => ui.Ngo)
                .Select(ui => ui.Ngo).ProjectToSummaryDto().ToArrayAsync(ct);
    }

    public async Task NotifyInterestedUsersAsync(WelfareProgramme programme, CancellationToken ct = default)
    {
        var interestedUsers = dbContext.UserInterests.AsNoTracking().Include(ui => ui.Ngo)
                .Where(ui => ui.NgoId == programme.NgoId).Select(ui => new { ui.UserId, ui.Ngo }).AsAsyncEnumerable();

        await foreach (var interest in interestedUsers)
        {
            await notificationService.CreateNotificationAsync(
                interest.UserId,
                $"New Programme launched by {interest.Ngo.Name}: {programme.Name}",
                ct);
        }
    }
}
