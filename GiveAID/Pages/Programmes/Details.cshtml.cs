using System.ComponentModel.DataAnnotations;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Programmes;

public class ProgrammeDetailsModel(IProgrammeService programmeService, IEmailService emailService) : PageModel
{
    public ProgrammeDto? Programme { get; set; }

    [BindProperty]
    public InviteFriendModel InviteForm { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Programme = await programmeService.GetProgrammeByIdAsync(id);

        if (Programme == null) { return NotFound(); }

        var url = Url.Page("/Programmes/Details", pageHandler: null, values: new { id }, protocol: Request.Scheme);
        
        InviteForm = new InviteFriendModel
        {
            Subject = $"Join me in supporting {Programme.Name}!",
            Body = $"Hi,\n\nI just found this amazing programme \"{Programme.Name}\" by {Programme.NgoName}. They are raising funds to help people and I think you might be interested in supporting this cause too.\n\nYou can learn more and donate here: {url}\n\nLet's make a difference together!\n\nBest,"
        };

        return Page();
    }

    public async Task<IActionResult> OnPostInviteAsync(Guid id)
    {
        if (!ModelState.IsValid)
        {
            Programme = await programmeService.GetProgrammeByIdAsync(id);
            return Page();
        }

        // Convert newlines to HTML breaks since EmailService sends as HTML
        var htmlBody = InviteForm.Body.Replace("\n", "<br/>");

        await emailService.SendEmailAsync(InviteForm.FriendEmail, InviteForm.Subject, htmlBody);
        
        TempData["SuccessMessage"] = "Invitation sent successfully!";
        return RedirectToPage(new { id });
    }
}

public class InviteFriendModel
{
    [Required(ErrorMessage = "Please provide your friend's email address.")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
    public string FriendEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a subject.")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please provide a message.")]
    public string Body { get; set; } = string.Empty;
}
