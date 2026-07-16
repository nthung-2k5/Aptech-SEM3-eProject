using System.Security.Claims;
using FluentValidation;
using GiveAID.Data;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Member;

[Authorize(Roles = "Member")]
public class ProfileModel(AppDbContext context, IValidator<ProfileModel.InputModel> validator, IAuthService authService) : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId)) { return RedirectToPage("/Login/Index"); }

        var user = await context.Users.FindAsync(userId);

        if (user == null) { return RedirectToPage("/Login/Index"); }

        Input = new InputModel
        {
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            Occupation = user.Occupation
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var result = await validator.ValidateAsync(Input);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError($"Input.{error.PropertyName}", error.ErrorMessage);
            }

            return Page();
        }

        string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdStr, out var userId)) { return RedirectToPage("/Login/Index"); }

        var user = await context.Users.FindAsync(userId);

        if (user == null) { return RedirectToPage("/Login/Index"); }

        user.FullName = Input.FullName;
        user.DateOfBirth = Input.DateOfBirth;
        user.Address = Input.Address;
        user.Occupation = Input.Occupation;

        await context.SaveChangesAsync();

        var tokenString = HttpContext.Request.Cookies["jwt_token"];
        DateTime? expires = null;
        if (!string.IsNullOrEmpty(tokenString))
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            if (handler.CanReadToken(tokenString))
            {
                var jwtToken = handler.ReadJwtToken(tokenString);
                expires = jwtToken.ValidTo;
            }
        }

        var newToken = await authService.RefreshTokenAsync(userId, expires);
        if (!string.IsNullOrEmpty(newToken))
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires.HasValue ? new DateTimeOffset(expires.Value) : DateTimeOffset.UtcNow.AddHours(24)
            };
            HttpContext.Response.Cookies.Append("jwt_token", newToken, cookieOptions);
        }

        TempData["SuccessMessage"] = "Your profile has been updated successfully.";

        return RedirectToPage();
    }

    public class Validator : AbstractValidator<InputModel>
    {
        public Validator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of birth must be in the past");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters");

            RuleFor(x => x.Occupation)
                .MaximumLength(50).WithMessage("Occupation cannot exceed 50 characters");
        }
    }
}
