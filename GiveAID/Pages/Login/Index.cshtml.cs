using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Login;

public class LoginModel : PageModel
{
    public bool HasError { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnGetLogout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostAsync(string email, string password)
    {
        if (email == "admin@give-aid.org")
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, "Admin")
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal);
        }
        else if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            HasError = true;
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
