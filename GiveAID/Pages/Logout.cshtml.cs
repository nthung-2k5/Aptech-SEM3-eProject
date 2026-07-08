using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages;

public class Logout : PageModel
{
    public async Task<IActionResult> OnPostAsync(CancellationToken ct = default)
    {
        await HttpContext.SignOutAsync();
        Response.Cookies.Delete("jwt_token");
        return RedirectToPage("/Index");
    }
}
