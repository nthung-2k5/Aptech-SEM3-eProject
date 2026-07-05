using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Register;

public class RegisterModel : PageModel
{
    public bool IsSuccess { get; set; }

    public void OnGet() { }

    public IActionResult OnPost(string firstName, string lastName, string email, string password,
                                string confirmPassword)
    {
        if (password == confirmPassword) { IsSuccess = true; }

        return Page();
    }
}
