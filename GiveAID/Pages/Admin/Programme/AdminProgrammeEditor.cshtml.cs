using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Programme;

public class AdminProgrammeEditor(
    IProgrammeService programmeService,
    INgoService ngoService,
    IDonationCauseService causeService,
    IValidator<AdminProgrammeEditor> validator) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
        public Guid NgoId { get; set; }
        public Guid CauseId { get; set; }
        public string Description { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string? Location { get; set; }
        public DateTimeOffset StartTime { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? EndTime { get; set; }
        public decimal? MaxDonation { get; set; }
    }

    public FormModel Form { get; set; } = new();
    public NgoSummaryDto[] AvailableNgos { get; set; } = [];
    public DonationCauseDto[] AvailableCauses { get; set; } = [];

    public override async Task MountAsync()
    {
        AvailableNgos = await ngoService.GetAllNgosAsync();
        AvailableCauses = await causeService.GetAllDonationCausesAsync();

        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var p = await programmeService.GetProgrammeSaveDtoByIdAsync(Id.Value);
            if (p != null)
            {
                Form = new FormModel
                {
                    Name = p.Name,
                    NgoId = p.NgoId,
                    CauseId = p.CauseId,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    Location = p.Location,
                    StartTime = p.StartTime,
                    EndTime = p.EndTime,
                    MaxDonation = p.MaxDonation
                };
            }
        }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        var saveDto = new ProgrammeSaveDto(
            Form.NgoId,
            Form.CauseId,
            Form.Name,
            Form.ImageUrl,
            Form.Description,
            Form.StartTime,
            Form.EndTime,
            Form.MaxDonation,
            Form.Location
        );

        if (Id.HasValue && Id.Value != Guid.Empty) { await programmeService.UpdateProgrammeAsync(Id.Value, saveDto); }
        else { await programmeService.CreateProgrammeAsync(saveDto); }

        Redirect(Url.Page("/Admin/Programme/Index"));
    }

    public class Validator : AbstractValidator<AdminProgrammeEditor>
    {
        public Validator()
        {
            RuleFor(x => x.Form.Name)
                .NotEmpty().WithMessage("Programme name is required")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters");

            RuleFor(x => x.Form.NgoId)
                .NotEqual(Guid.Empty).WithMessage("Please select an NGO");

            RuleFor(x => x.Form.CauseId)
                .NotEqual(Guid.Empty).WithMessage("Please select a cause");

            RuleFor(x => x.Form.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.Form.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required")
                .MaximumLength(2048).WithMessage("Image URL cannot exceed 2048 characters");

            RuleFor(x => x.Form.Location)
                .MaximumLength(255).WithMessage("Location cannot exceed 255 characters");

            RuleFor(x => x.Form.EndTime)
                .GreaterThan(x => x.Form.StartTime)
                .When(x => x.Form.EndTime.HasValue)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.Form.MaxDonation)
                .GreaterThan(0)
                .When(x => x.Form.MaxDonation.HasValue)
                .WithMessage("Max donation must be a positive value");
        }
    }
}
