using System.Security.Claims;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Ngo;

public class DetailsModel(
    INgoService ngoService, 
    IDonationCauseService donationCauseService,
    IUserInterestService userInterestService) : PageModel
{
    public NgoDto? Ngo { get; set; }
    public DonationCauseDto[] Causes { get; set; } = [];
    
    public bool IsFollowing { get; set; }

    [BindProperty]
    public Guid? SelectedCauseId { get; set; }

    [BindProperty]
    public decimal Amount { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Ngo = await ngoService.GetNgoByIdAsync(id);
        if (Ngo == null)
        {
            return NotFound();
        }

        Causes = await donationCauseService.GetAllDonationCausesAsync();

        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Member"))
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdStr, out var userId))
            {
                IsFollowing = await userInterestService.IsFollowingNgoAsync(userId, id);
            }
        }

        return Page();
    }
    
    public IActionResult OnPostDonate(Guid id)
    {
        return RedirectToPage("/Donate/Index", new { ngoId = id, causeId = SelectedCauseId, amount = Amount });
    }

    public async Task<IActionResult> OnPostToggleFollowAsync(Guid id)
    {
        if (User.Identity?.IsAuthenticated != true || !User.IsInRole("Member"))
        {
            return RedirectToPage("/Login/Index");
        }

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(userIdStr, out var userId))
        {
            var isFollowing = await userInterestService.IsFollowingNgoAsync(userId, id);
            if (isFollowing)
            {
                await userInterestService.UnfollowNgoAsync(userId, id);
            }
            else
            {
                await userInterestService.FollowNgoAsync(userId, id);
            }
        }
        
        return RedirectToPage(new { id });
    }
}
