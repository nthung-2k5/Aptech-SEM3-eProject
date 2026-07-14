using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Register;

public class RegisterForm(
    IMemberService memberService,
    IAuthService authService,
    IValidator<RegisterForm> validator) : HydroComponent
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Occupation { get; set; }
    public string? Address { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public bool Agree { get; set; }

    public bool IsSuccess { get; set; }

    public async Task Submit()
    {
        if (!this.Validate(validator)) { return; }

        var user = new MemberCreateDto(
            Name,
            Email,
            Password,
            DateOfBirth.Value,
            Address ?? string.Empty,
            Phone,
            Occupation);

        try
        {
            await memberService.CreateMemberAsync(user);
            IsSuccess = true;
            
            var result = await authService.LoginAsync(Email, Password);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            };

            HttpContext.Response.Cookies.Append("jwt_token", result.Token, cookieOptions);
            
            Redirect(Url.Page("/Index"));
        }
        catch (DuplicateException ex) when (ex.FieldName == nameof(Email))
        {
            ModelState.AddModelError(nameof(Email), "An account with this email already exists.");
        }
        catch (DuplicateException ex) when (ex.FieldName == nameof(Phone))
        {
            ModelState.AddModelError(nameof(Phone), "An account with this phone number already exists.");
        }
    }

    public class Validator : AbstractValidator<RegisterForm>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required")
                .PhoneNumber().WithMessage("Phone number must be in E.164 format");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Today)).WithMessage("Date of Birth must be in the past");
            
            RuleFor(x => x.Occupation)
                .NotEmpty().WithMessage("Occupation is required")
                .MaximumLength(50).WithMessage("Occupation cannot exceed 50 characters");

            RuleFor(x => x.Address)
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Min 6 characters");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.Password).WithMessage("Passwords do not match");

            RuleFor(x => x.Agree)
                .Equal(true).WithMessage("You must accept the terms");
        }
    }
}
