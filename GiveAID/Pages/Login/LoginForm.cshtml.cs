using System.ComponentModel.DataAnnotations;
using GiveAID.Models;
using GiveAID.Services;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Login;

public class LoginForm(IAuthService authService) : HydroComponent
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    public bool HasError { get; set; }

    public async Task Submit()
    {
        if (!Validate()) { return; }

        try
        {
            var result = await authService.LoginAsync(Email, Password);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(24)
            };

            HttpContext.Response.Cookies.Append("jwt_token", result.Token, cookieOptions);

            Redirect(result.Role == UserRole.Admin ? "/Admin" : "/");
        }
        catch (LoginException ex) { HasError = true; }
    }
}
