using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class NotificationService(AppDbContext context) : INotificationService
{
    public async Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Notifications.Where(n => n.UserId == userId && !n.IsRead).CountAsync(ct);
    }

    public async Task<NotificationDto[]> GetUserNotificationsAsync(Guid userId, int limit = 10, CancellationToken ct = default)
    {
        return await context.Notifications
                .Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedAt)
                .Take(limit)
                .ProjectToDto().ToArrayAsync(ct);
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken ct = default)
    {
        await context.Notifications.Where(n => n.NotificationId == notificationId)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);
    }

    public async Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default)
    {
        await context.Notifications.Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), ct);
    }

    public async Task CreateNotificationAsync(Guid userId, string content, CancellationToken ct = default)
    {
        context.Notifications.Add(new Notification
        {
            UserId = userId,
            Content = content
        });
        await context.SaveChangesAsync(ct);
    }
}
