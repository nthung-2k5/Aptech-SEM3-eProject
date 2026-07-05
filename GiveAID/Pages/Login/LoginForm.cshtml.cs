using System.ComponentModel.DataAnnotations;
using Hydro;

namespace GiveAID.Pages.Login;

public class LoginForm : HydroComponent
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public bool HasError { get; set; }

    public void Submit()
    {
        if (!Validate()) { return; }

        if (ModelState.IsValid)
        {
            if (Email == "admin@give-aid.org") { Redirect("/Admin"); }
            else if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password)) { HasError = true; }
            else { Redirect("/"); }
        }
    }
}
