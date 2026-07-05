using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.AboutUs;

// [Authorize(Roles = "Admin")]
public class EditorModel(IAboutUsSubpageService aboutUsService) : PageModel
{
    public AboutUsSubpageDetailsDto? CurrentPage { get; set; }
    public bool IsEditMode { get; set; }

    public async Task<IActionResult> OnGetAsync(string? slug, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(slug))
        {
            IsEditMode = true;
            CurrentPage = await aboutUsService.GetBySlugAsync(slug, ct);

            if (CurrentPage == null) { return RedirectToPage("/AboutUs/Index"); }
        }
        else { IsEditMode = false; }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? originalSlug, string slug, string title, string content,
                                                 CancellationToken ct)
    {
        var page = new AboutUsSubpageDetailsDto(title, slug, content);

        if (!string.IsNullOrEmpty(originalSlug)) { await aboutUsService.UpdateSubpageAsync(page, ct); }
        else { await aboutUsService.AddSubpageAsync(page, ct); }

        return RedirectToPage("/AboutUs/Index", new { slug });
    }
}
