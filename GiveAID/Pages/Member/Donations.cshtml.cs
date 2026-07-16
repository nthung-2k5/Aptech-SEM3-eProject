using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Member;

[Authorize(Roles = "Member")]
public class DonationsModel : PageModel
{
    public void OnGet()
    {
    }
}
