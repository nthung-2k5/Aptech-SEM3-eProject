using GiveAID.Dtos;
using GiveAID.Models;

namespace GiveAID.Services.Abstractions;

public interface IUserInterestService
{
    Task FollowNgoAsync(Guid userId, Guid ngoId, CancellationToken ct = default);
    Task UnfollowNgoAsync(Guid userId, Guid ngoId, CancellationToken ct = default);
    Task<bool> IsFollowingNgoAsync(Guid userId, Guid ngoId, CancellationToken ct = default);
    Task<NgoSummaryDto[]> GetUserInterestsAsync(Guid userId, CancellationToken ct = default);
    Task NotifyInterestedUsersAsync(WelfareProgramme programme, CancellationToken ct = default);
}
