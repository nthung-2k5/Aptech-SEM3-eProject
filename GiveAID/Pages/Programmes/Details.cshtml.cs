using System.ComponentModel.DataAnnotations;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using GiveAID.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Programmes;

public class ProgrammeDetailsModel(IBackgroundTaskQueue backgroundTaskQueue, IProgrammeService programmeService, IEmailService emailService) : PageModel
{
    public ProgrammeDto? Programme { get; set; }

    [BindProperty]
    public InviteFriendModel InviteForm { get; set; } = new();

    // Giữ lại thuộc tính này nếu file .cshtml của bạn cần gọi @Model.Html
    public string Html { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Programme = await programmeService.GetProgrammeByIdAsync(id);

        if (Programme == null) { return NotFound(); }

        var url = Url.Page("/Programmes/Details", pageHandler: null, values: new { id }, protocol: Request.Scheme);

        InviteForm = new InviteFriendModel
        {
            // Khởi tạo danh sách email rỗng
            FriendEmails = [],
            Subject = $"Join me in supporting {Programme.Name}!",
            Body = $"Hi,\n\nI just found this amazing programme \"{Programme.Name}\" by {Programme.NgoName}. They are raising funds to help people and I think you might be interested in supporting this cause too.\n\nYou can learn more and donate here: {url}\n\nLet's make a difference together!\n\nBest,"
        };

        return Page();
    }

    public async Task<IActionResult> OnPostInviteAsync(Guid id)
    {
        // 1. Kiểm tra Validate của Form
        if (!ModelState.IsValid)
        {
            Programme = await programmeService.GetProgrammeByIdAsync(id);
            if (Programme == null) return NotFound();
            return Page();
        }

        // 2. Lấy lại thông tin Programme 
        Programme = await programmeService.GetProgrammeByIdAsync(id);
        if (Programme == null) { return NotFound(); }

        // 3. Xử lý nội dung email
        string? url = Url.Page("/Programmes/Details", pageHandler: null, values: new { id }, protocol: Request.Scheme);
        string formattedBody = InviteForm.Body.Replace("\n", "<br/>");

        string htmlBody = $"""
            <!DOCTYPE html>
            <html>
              <head>
                <meta charset='UTF-8'>
              </head>
              <body style='margin:0;padding:0;background:#f4f7fb;font-family:Segoe UI,Arial,sans-serif;'>
                <table width='100%' cellpadding='0' cellspacing='0' style='background:#1A5C6B;padding:30px 0;'>
                  <tr>
                    <td align='center'>
                      <table width='600' cellpadding='0' cellspacing='0' style='background:#ffffff;border-radius:12px;overflow:hidden;'>
                        <tr>
                          <td style='background:#0d6efd;padding:30px;text-align:center;color:white;'>
                            <h1>🤝 You're Invited!</h1>
                          </td>
                        </tr>
                        <tr>
                          <td style='padding:30px;'>
                            <p>{formattedBody}</p>
                            <hr/>
                            <p><strong>Programme:</strong> {Programme.Name}</p>
                            <p><strong>Organization:</strong> {Programme.NgoName}</p>
                            <p style='text-align:center;margin-top:30px'>
                              <a href='{url}' style='background:#0d6efd;padding:12px 24px;color:white;text-decoration:none;border-radius:6px'>
                              ❤️ Learn More & Donate
                              </a>
                            </p>
                          </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
              </body>
            </html>
            """;

        // 4. Lọc danh sách email từ giao diện (bỏ các ô trống, cắt khoảng trắng dư và bỏ email trùng)
        var inputEmails = InviteForm.FriendEmails
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .Select(email => email.Trim())
            .Distinct()
            .ToList();

        //  5. Giới hạn số lượng email gửi đi (ví dụ: tối đa 3 email mỗi lần)
        int maxEmailsAllowed = 3;
        if (inputEmails.Count > maxEmailsAllowed)
        {
            ModelState.AddModelError("InviteForm.FriendEmails", $"You can only invite up to {maxEmailsAllowed} friends at a time.");

            Programme = await programmeService.GetProgrammeByIdAsync(id);
            if (Programme == null) return NotFound();
            return Page(); // Render lại trang và báo lỗi
        }

        // 1. Copy dữ liệu ra các biến cục bộ để tránh lỗi capture 'this'
        var emailsToProcess = inputEmails.ToList();
        var subject = InviteForm.Subject;
        var content = htmlBody;

        // 2. Ném việc vào Background Queue
        await backgroundTaskQueue.QueueBackgroundWorkItemAsync(async (serviceProvider, ct) =>
        {
            // CẢNH BÁO: Không dùng TempData, ModelState, hay Request ở trong này!
            var backgroundEmailService = serviceProvider.GetRequiredService<IEmailService>();
            var logger = serviceProvider.GetRequiredService<ILogger<ProgrammeDetailsModel>>(); // Lấy logger để ghi lỗi nếu cần

            foreach (var email in emailsToProcess)
            {
                try
                {
                    // Sử dụng các biến cục bộ (subject, content) đã copy ở trên
                    await backgroundEmailService.SendEmailAsync(email, subject, content, ct);
                }
                catch (Exception ex)
                {
                    // Chạy ngầm thì chỉ có thể ghi Log vào file/console chứ không báo lên màn hình được nữa
                    logger.LogError(ex, $"Failed to send background email to {email}");
                }
            }
        });

        // 3. Khai báo TempData ở luồng chính (Main Thread) NGAY TRƯỚC KHI TRẢ VỀ
        TempData["SuccessMessage"] = $"Your invitations to {inputEmails.Count} friends are being sent in the background!";

        return RedirectToPage(new { id });
    }
}

// Cập nhật Model chứa List thay vì String
public class InviteFriendModel
{
    [Required(ErrorMessage = "Please provide at least one friend's email address.")]
    public List<string> FriendEmails { get; set; } = new();

    [Required(ErrorMessage = "Please provide a subject.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a message.")]
    public string Body { get; set; } = string.Empty;
}