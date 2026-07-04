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
            var claims = new List<System.Security.Claims.Claim>
            {
                new(System.Security.Claims.ClaimTypes.Email, email),
                new(System.Security.Claims.ClaimTypes.Role, "Admin")
            };
            var identity = new System.Security.Claims.ClaimsIdentity(claims, "Cookies");
            var principal = new System.Security.Claims.ClaimsPrincipal(identity);

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
