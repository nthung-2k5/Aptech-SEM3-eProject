using FluentValidation;
using Hydro;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;

namespace GiveAID.Pages.Components;

public class ContactForm(IValidator<ContactForm> validator, IUserQueryService userQueryService, IHttpContextAccessor httpContextAccessor) : HydroComponent
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }

    public bool IsSuccess { get; set; }
    public bool IsMember { get; set; }

    public override void Mount()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true && user.IsInRole("Member"))
        {
            FullName = user.Identity.Name ?? "";
            Email = user.FindFirstValue(ClaimTypes.Email) ?? "";
            IsMember = true;
        }
        else
        {
            IsMember = false;
        }
    }

    public async Task Submit()
    {
        if (!IsMember) { return; }
        if (!this.Validate(validator)) { return; }
        var user = httpContextAccessor.HttpContext?.User;
        var id = Guid.Parse(user!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        IsSuccess = await userQueryService.CreateQueryAsync(new UserQueryCreateDto(id, Subject, Message));
    }

    public void Reset()
    {
        IsSuccess = false;
        Subject = string.Empty;
        Message = string.Empty;
    }

    public class Validator : AbstractValidator<ContactForm>
    {
        public Validator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full Name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Address is required")
                .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required")
                .MaximumLength(200).WithMessage("Subject cannot exceed 200 characters");

            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("Message is required")
                .MaximumLength(2000).WithMessage("Message cannot exceed 2000 characters");
        }
    }
}
