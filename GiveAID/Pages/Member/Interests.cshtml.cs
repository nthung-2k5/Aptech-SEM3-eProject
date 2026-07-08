using System.Security.Claims;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Member;

[Authorize(Roles = "Member")]
public class InterestsModel(IUserInterestService userInterestService) : PageModel
{
    public NgoSummaryDto[] Interests { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return RedirectToPage("/Login/Index");

        Interests = await userInterestService.GetUserInterestsAsync(userId);

        return Page();
    }

    public async Task<IActionResult> OnPostUnfollowAsync(Guid ngoId)
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return RedirectToPage("/Login/Index");

        await userInterestService.UnfollowNgoAsync(userId, ngoId);

        return RedirectToPage();
    }
}
