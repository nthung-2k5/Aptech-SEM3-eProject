using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.AboutUs;

public class AboutModel : PageModel
{
    public string? Slug { get; set; }

    public void OnGet(string? slug)
    {
        Slug = slug;
    }
}
