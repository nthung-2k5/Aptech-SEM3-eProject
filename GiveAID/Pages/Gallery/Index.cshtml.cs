using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages;

public class GalleryModel : PageModel
{
    public string Filter { get; set; } = "All";

    public void OnGet(string filter = "All") { Filter = filter; }
}
