using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.AboutUs;

public class AboutModel(IAboutUsSubpageService aboutUsService) : PageModel
{
    public AboutUsSubpageSummaryDto[] Subpages { get; set; } = [];
    public AboutUsSubpageDto? CurrentPage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? slug, CancellationToken ct)
    {
        Subpages = await aboutUsService.ListSubpagesAsync(ct);

        if (Subpages.Length == 0) { return Page(); }

        if (string.IsNullOrEmpty(slug)) { CurrentPage = await aboutUsService.GetBySlugAsync(Subpages[0].Slug, ct); }
        else
        {
            CurrentPage = await aboutUsService.GetBySlugAsync(slug, ct) ??
                          await aboutUsService.GetBySlugAsync(Subpages[0].Slug, ct);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string slug, CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == true) { await aboutUsService.DeleteSubpageAsync(slug, ct); }

        return RedirectToPage("/AboutUs/Index", new { slug = "" });
    }
}
