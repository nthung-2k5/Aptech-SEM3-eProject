using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Admin.DonationCause;

public class EditorModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid? Id { get; set; }

    public void OnGet()
    {
    }
}
