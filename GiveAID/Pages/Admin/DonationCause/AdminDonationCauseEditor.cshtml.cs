using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Hydro;
using Hydro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.DonationCause;

public class AdminDonationCauseEditor(
    IDonationCauseService donationCauseService,
    IValidator<AdminDonationCauseEditor> validator) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
    }

    public FormModel Form { get; set; } = new();

    public override async Task MountAsync()
    {
        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var cause = await donationCauseService.GetDonationCauseByIdAsync(Id.Value);

            if (cause != null)
            {
                Form = new FormModel
                {
                    Name = cause.Name
                };
            }
        }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        var saveDto = new DonationCauseSaveDto(Form.Name);

        try
        {
            if (Id.HasValue && Id.Value != Guid.Empty) { await donationCauseService.UpdateDonationCauseAsync(Id.Value, saveDto); }
            else { await donationCauseService.CreateDonationCauseAsync(saveDto); }
        }
        catch (DuplicateException)
        {
            ModelState.AddModelError("Form.Name", "A cause with this name already exists.");
            return;
        }

        Redirect(Url.Page("/Admin/DonationCause/Index"));
    }

    public class Validator : AbstractValidator<AdminDonationCauseEditor>
    {
        public Validator()
        {
            RuleFor(x => x.Form.Name)
                .NotEmpty().WithMessage("Cause name is required")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters");
        }
    }
}
