using System.Security.Claims;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages;

[Authorize(Roles = "Member")]
public class NotificationsModel(INotificationService notificationService) : PageModel
{
    public NotificationDto[] Notifications { get; private set; } = [];
    public int UnreadCount { get; private set; }

    public async Task<IActionResult> OnGetAsync()
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId)) { return RedirectToPage("/Register/Index"); }

        Notifications = await notificationService.GetUserNotificationsAsync(userId, 50);
        UnreadCount = Notifications.Count(n => !n.IsRead);
        return Page();
    }

    public async Task<IActionResult> OnPostMarkReadAsync(Guid id)
    {
        await notificationService.MarkAsReadAsync(id);
        string referer = Request.Headers.Referer.ToString();
        return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
    }

    public async Task<IActionResult> OnPostMarkAllReadAsync()
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userIdStr, out var userId)) { await notificationService.MarkAllAsReadAsync(userId); }

        string referer = Request.Headers.Referer.ToString();
        return Redirect(string.IsNullOrEmpty(referer) ? "/" : referer);
    }
}
