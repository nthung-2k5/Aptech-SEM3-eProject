using FluentValidation;
using GiveAID.Models;
using GiveAID.Services;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Login;

public class LoginForm(
    IAuthService authService,
    IValidator<LoginForm> validator) : HydroComponent
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }

    public string? ErrorMessage { get; set; }

    public async Task Submit()
    {
        ErrorMessage = null;
        
        if (!this.Validate(validator)) { return; }

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
        catch (LoginException ex)
        {
            ErrorMessage = ex.Message;
        }
    }

    public class Validator : AbstractValidator<LoginForm>
    {
        public Validator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
