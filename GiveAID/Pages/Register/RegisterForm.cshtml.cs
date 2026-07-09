using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Register;

public class RegisterForm(
    IMemberService memberService,
    IValidator<RegisterForm> validator) : HydroComponent
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public string Address { get; set; }
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
            DateOnly.Parse(DateOfBirth),
            Address,
            Phone,
            Occupation ?? string.Empty);

        await memberService.CreateMemberAsync(user);

        // if (result.Success)
        // {
        //     IsSuccess = true; 
        // }
        // else
        // {
        //     // Optionally handle registration errors here (e.g. email already registered)
        // }
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
                .Matches(@"^\d{10}$").WithMessage("Enter 10-digit phone number");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required");

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
