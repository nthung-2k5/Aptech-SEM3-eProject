using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Services;

public class UserQueryService(
    AppDbContext dbContext,
    IEmailService emailService,
    INotificationService notificationService
) : IUserQueryService
{
    public async Task<UserQueryDto[]> GetAllQueriesAsync(CancellationToken ct = default)
    {
        return await dbContext.UserQueries.AsNoTracking().Include(q => q.User).OrderByDescending(q => q.CreatedAt)
                .Select(q => new UserQueryDto(
                    q.QueryId,
                    q.UserId,
                    q.User.FullName,
                    q.Subject,
                    q.MessageText,
                    q.ReplyText,
                    q.CreatedAt)).ToArrayAsync(ct);
    }

    public async Task<UserQueryDto[]> GetUnansweredQueriesAsync(CancellationToken ct = default)
    {
        return await dbContext.UserQueries.AsNoTracking().Include(q => q.User).Where(q => q.ReplyText == null)
                .OrderByDescending(q => q.CreatedAt).Select(q => new UserQueryDto(
                    q.QueryId,
                    q.UserId,
                    q.User.FullName,
                    q.Subject,
                    q.MessageText,
                    q.ReplyText,
                    q.CreatedAt)).ToArrayAsync(ct);
    }

    public async Task<bool> CreateQueryAsync(UserQueryCreateDto query, CancellationToken ct = default)
    {
        var entity = new UserQuery
        {
            UserId = query.UserId,
            Subject = query.Subject,
            MessageText = query.MessageText
        };

        dbContext.UserQueries.Add(entity);
        int rows = await dbContext.SaveChangesAsync(ct);
        return rows > 0;
    }

    public async Task<bool> ReplyQueryAsync(Guid id, string replyText, CancellationToken ct = default)
    {
        var query = await dbContext.UserQueries.Include(q => q.User)
                .FirstOrDefaultAsync(q => q.QueryId == id && q.ReplyText == null, ct);

        if (query == null) { return false; }

        query.ReplyText = replyText;
        int rows = await dbContext.SaveChangesAsync(ct);

        if (rows > 0 && !string.IsNullOrWhiteSpace(query.User.Email))
        {
            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px;'>
                    <h2 style='color: #2c3e50;'>Hello {query.User.FullName},</h2>
                    <p style='color: #34495e; font-size: 16px;'>Thank you for reaching out. We have reviewed and responded to your query regarding <strong>{query.Subject}</strong>.</p>
                    
                    <div style='background-color: #f9f9f9; padding: 15px; border-left: 4px solid #3498db; margin: 20px 0;'>
                        <h4 style='margin-top: 0; color: #2980b9;'>Your Query:</h4>
                        <p style='color: #555; white-space: pre-wrap;'>{query.MessageText}</p>
                    </div>

                    <div style='background-color: #f4fff8; padding: 15px; border-left: 4px solid #2ecc71; margin: 20px 0;'>
                        <h4 style='margin-top: 0; color: #27ae60;'>Our Reply:</h4>
                        <p style='color: #555; white-space: pre-wrap;'>{replyText}</p>
                    </div>
                    
                    <p style='color: #34495e; font-size: 14px;'>If you have any further questions, please do not hesitate to contact us again.</p>
                    <br/>
                    <p style='color: #7f8c8d; font-size: 12px; border-top: 1px solid #eee; padding-top: 10px;'>Best Regards,<br/><strong>GiveAID Support Team</strong></p>
                </div>";

            await emailService.SendEmailAsync(query.User.Email, $"Re: {query.Subject}", emailBody, ct);

            await notificationService.CreateNotificationAsync(
                query.UserId,
                $"Your query regarding '{query.Subject}' has been answered. Please check your email for the response.",
                ct);
        }

        return rows > 0;
    }
}
