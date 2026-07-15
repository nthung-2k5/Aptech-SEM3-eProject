using System.Security.Claims;
using GiveAID.Data;
using GiveAID.Dtos;
using GiveAID.Services;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Pages.Member;

[Authorize(Roles = "Member")]
public class DonationsModel(IDonationService service) : PageModel
{
    public UserDonationDto[] Donations { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken ct = default)
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId)) { return RedirectToPage("/Register/Index"); }

        Donations = await service.GetDonationsByUserAsync(userId, ct);

        return Page();
    }
}
