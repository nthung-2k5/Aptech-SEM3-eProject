using GiveAID.Models;

namespace GiveAID.Dtos;

public record NotificationDto(Guid Id, string Content, DateTimeOffset CreatedAt, bool IsRead);

public static class NotificationMapper
{
    extension(Notification notification)
    {
        public NotificationDto ToDto() => new(notification.NotificationId, notification.Content, notification.CreatedAt, notification.IsRead);
    }

    extension(IQueryable<Notification> notifications)
    {
        public IQueryable<NotificationDto> ProjectToDto() =>
            notifications.Select(n => new NotificationDto(n.NotificationId, n.Content, n.CreatedAt, n.IsRead));
    }
}
