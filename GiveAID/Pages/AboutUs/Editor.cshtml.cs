using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.AboutUs;

[Authorize(Roles = nameof(UserRole.Admin))]
public class EditorModel(IAboutUsSubpageService aboutUsService) : PageModel
{
    public AboutUsSubpageDto? CurrentPage { get; set; }
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

    public async Task<IActionResult> OnPostAsync(string? originalSlug, string slug, string title, string? content,
                                                 CancellationToken ct)
    {
        var page = new AboutUsSubpageDto(slug, title, content ?? string.Empty);

        try
        {
            if (!string.IsNullOrEmpty(originalSlug)) { await aboutUsService.UpdateSubpageAsync(page, ct); }
            else { await aboutUsService.AddSubpageAsync(page, ct); }

            return RedirectToPage("/AboutUs/Index", new { slug });
        }
        catch (DuplicateException ex)
        {
            switch (ex.FieldName)
            {
                case nameof(AboutUsSubpageDto.Title):
                    ModelState.AddModelError(nameof(title), ex.Message);
                    break;
                default:
                    ModelState.AddModelError(ex.FieldName, ex.Message);
                    break;
            }
            
            IsEditMode = !string.IsNullOrEmpty(originalSlug);
            CurrentPage = page;
            return Page();
        }
    }
}
