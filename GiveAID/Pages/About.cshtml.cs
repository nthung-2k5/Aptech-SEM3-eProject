using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages;

public class AboutModel(IAboutUsSubpageService aboutUsService) : PageModel
{
    public AboutUsSubpageDto[] Subpages { get; set; } = [];
    public AboutUsSubpageDto? CurrentPage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? slug, CancellationToken ct)
    {
        Subpages = await aboutUsService.ListSubpagesAsync(ct);
        if (Subpages.Length == 0) return Page();

        if (string.IsNullOrEmpty(slug))
        {
            CurrentPage = Subpages.First();
        }
        else
        {
            CurrentPage = aboutUsService.GetBySlugAsync(slug, ct) ?? Subpages.First();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCreateAsync(string title, string slug, string content, CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            await aboutUsService.AddSubpageAsync(new AboutUsSubpageDto(title, slug, content), ct);
        }
        
        return RedirectToPage(new { slug });
    }

    public async Task<IActionResult> OnPostUpdateAsync(string title, string slug, string content, CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            var page = new AboutUsSubpageDto(title, slug, content);
            await aboutUsService.UpdateSubpageAsync(page, ct);
        }
        
        return RedirectToPage(new { slug });
    }

    public async Task<IActionResult> OnPostDeleteAsync(string slug, CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            await aboutUsService.DeleteSubpageAsync(slug, ct);
        }
        
        return RedirectToPage("/About", new { slug = "" });
    }
}
