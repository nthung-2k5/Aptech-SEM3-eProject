using GiveAID.Dtos;

namespace GiveAID.Services.Abstractions;

public interface INotificationService
{
    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);
    Task<NotificationDto[]> GetUserNotificationsAsync(Guid userId, int limit = 10, CancellationToken ct = default);
    Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default);
    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);
    Task CreateNotificationAsync(Guid userId, string content, CancellationToken ct = default);
}
