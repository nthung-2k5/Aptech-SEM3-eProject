using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Member;

public class AdminMemberEditor(
    IMemberService memberService,
    IValidator<AdminMemberEditor> validator) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Password { get; set; } = null;
        public string Address { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Occupation { get; set; } = "";
        public DateOnly DateOfBirth { get; set; } = new(1990, 1, 1);
    }

    public FormModel Form { get; set; } = new();

    public override async Task MountAsync()
    {
        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var m = await memberService.GetMemberByIdAsync(Id.Value);

            if (m != null)
            {
                Form = new FormModel
                {
                    FullName = m.FullName, Email = m.Email, Address = m.Address,
                    PhoneNumber = m.PhoneNumber, Occupation = m.Occupation, DateOfBirth = m.DateOfBirth
                };
            }
        }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        try
        {
            if (Id.HasValue && Id.Value != Guid.Empty)
            {
                var updateDto = new MemberUpdateDto(
                    Form.FullName,
                    Form.Email,
                    Form.Password,
                    Form.DateOfBirth,
                    Form.Address,
                    Form.PhoneNumber,
                    Form.Occupation);

                await memberService.UpdateMemberAsync(Id.Value, updateDto);
            }
            else
            {
                var createDto = new MemberCreateDto(
                    Form.FullName,
                    Form.Email,
                    Form.Password!,
                    Form.DateOfBirth,
                    Form.Address,
                    Form.PhoneNumber,
                    Form.Occupation);

                await memberService.CreateMemberAsync(createDto);
            }

            Redirect(Url.Page("/Admin/Member/Index"));
        }
        catch (DuplicateException ex)
        {
            switch (ex.FieldName)
            {
                case nameof(FormModel.Email):
                    ModelState.AddModelError($"Form.{nameof(FormModel.Email)}", "An account with this email already exists.");
                    break;
                case nameof(FormModel.PhoneNumber):
                    ModelState.AddModelError($"Form.{nameof(FormModel.PhoneNumber)}", "An account with this phone number already exists.");
                    break;
                default:
                    ModelState.AddModelError($"Form.{ex.FieldName}", ex.Message);
                    break;
            }
        }
    }

    public class Validator : AbstractValidator<AdminMemberEditor>
    {
        public Validator()
        {
            RuleFor(x => x.Form.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Form.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Enter a valid email address")
                .MaximumLength(255).WithMessage("Email cannot exceed 255 characters");

            RuleFor(x => x.Form.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .PhoneNumber().WithMessage("Phone number must be in E.164 format");

            RuleFor(x => x.Form.Address)
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters");

            RuleFor(x => x.Form.Occupation)
                .MaximumLength(50).WithMessage("Occupation cannot exceed 50 characters");

            RuleFor(x => x.Form.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .When(x => !string.IsNullOrWhiteSpace(x.Form.Password));

            RuleFor(x => x.Form.Password)
                .NotEmpty().WithMessage("Password is required for new members")
                .When(x => !x.Id.HasValue || x.Id.Value == Guid.Empty);

            RuleFor(x => x.Form.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required")
                .LessThan(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Date of birth must be in the past");
        }
    }
}
