using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Hydro;
using Hydro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Programme;

public class AdminProgrammeEditor(
    IProgrammeService programmeService,
    INgoService ngoService,
    IDonationCauseService causeService,
    IImageService imageService,
    IValidator<AdminProgrammeEditor> validator
) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
        public Guid NgoId { get; set; }
        public Guid CauseId { get; set; }
        public string Description { get; set; } = "";

        /// <summary>Either typed manually or populated by BindAsync when a file is selected.</summary>
        public string ImageUrl { get; set; } = "";

        public string? Location { get; set; }
        public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly? EndDate { get; set; }
        public decimal? MaxDonation { get; set; }
        public string? NewImageExtension { get; set; }
    }

    public FormModel Form { get; set; } = new();
    public NgoSummaryDto[] AvailableNgos { get; set; } = [];
    public DonationCauseDto[] AvailableCauses { get; set; } = [];

    /// <summary>Preview data-URL shown below the file picker. Set in BindAsync; not persisted in Hydro state.</summary>
    public string? PreviewImageSource { get; set; }

    [Transient]
    public IFormFile? ImageFile { get; set; }

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
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    MaxDonation = p.MaxDonation
                };
                PreviewImageSource = p.ImageUrl;
            }
        }
    }

    public override async Task BindAsync(PropertyPath property, object value)
    {
        if (property.Name == nameof(ImageFile))
        {
            var image = (IFormFile)value;
            PreviewImageSource = await image.ToDataUrlAsync();
            Form.NewImageExtension = Path.GetExtension(image.FileName);
        }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        if (Form.NewImageExtension != null)
        {
            string base64Data = PreviewImageSource!.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64Data);

            if (Id.HasValue && Id.Value != Guid.Empty && PreviewImageSource.StartsWith("data:"))
            {
                Form.ImageUrl = await imageService.UpdateImageAsync(Form.ImageUrl, bytes);
            }
            else { Form.ImageUrl = await imageService.UploadImageAsync("programmes", Form.NewImageExtension, bytes); }
        }

        var saveDto = new ProgrammeSaveDto(
            Form.NgoId,
            Form.CauseId,
            Form.Name,
            Form.ImageUrl,
            Form.Description,
            Form.StartDate,
            Form.EndDate,
            Form.MaxDonation,
            Form.Location);

        try
        {
            if (Id.HasValue && Id.Value != Guid.Empty)
            {
                await programmeService.UpdateProgrammeAsync(Id.Value, saveDto);
            }
            else { await programmeService.CreateProgrammeAsync(saveDto); }

            Redirect(Url.Page("/Admin/Programme/Index"));
        }
        catch (MissingForeignEntityException ex)
        {
            switch (ex.ReferenceField)
            {
                case nameof(ProgrammeSaveDto.NgoId):
                    ModelState.AddModelError($"Form.{ex.ReferenceField}", "Selected NGO does not exist");
                    break;
                case nameof(ProgrammeSaveDto.CauseId):
                    ModelState.AddModelError($"Form.{ex.ReferenceField}", "Selected cause does not exist");
                    break;
                default:
                    ModelState.AddModelError($"Form.{ex.ReferenceField}", ex.Message);
                    break;
            }
        }
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

            RuleFor(x => x.PreviewImageSource)
                .NotNull().WithMessage("Image file is required")
                .OverridePropertyName(nameof(Form.ImageUrl));

            RuleFor(x => x.Form.Location)
                .MaximumLength(255).WithMessage("Location cannot exceed 255 characters");

            RuleFor(x => x.Form.StartDate)
                .GreaterThanOrEqualTo(x => DateOnly.FromDateTime(DateTime.Now))
                .When(x => !x.Id.HasValue || x.Id.Value == Guid.Empty)
                .WithMessage("Start date cannot be in the past");
                
            RuleFor(x => x.Form.EndDate)
                .GreaterThan(x => x.Form.StartDate)
                .When(x => x.Form.EndDate.HasValue)
                .WithMessage("End time must be after start time");

            RuleFor(x => x.Form.MaxDonation)
                .GreaterThan(0)
                .When(x => x.Form.MaxDonation.HasValue)
                .WithMessage("Max donation must be a positive value");
        }
    }
}
