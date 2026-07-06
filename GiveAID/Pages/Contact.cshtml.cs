using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages;

public class ContactModel : PageModel
{
    public bool IsSuccess { get; set; }

    public void OnGet() { }

    public IActionResult OnPost(string name, string email, string subject, string message)
    {
        if (ModelState.IsValid) { IsSuccess = true; }

        return Page();
    }
}
