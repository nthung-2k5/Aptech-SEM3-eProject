using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Hydro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Partner;

public class AdminPartnerEditor(
    IPartnerService partnerService,
    IImageService imageService,
    IValidator<AdminPartnerEditor> validator) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
        /// <summary>Either typed manually or populated when a file is uploaded via BindAsync.</summary>
        public string LogoUrl { get; set; } = "";
        public string WebsiteLink { get; set; } = "";
        public string? NewImageExtension { get; set; }
    }

    public FormModel Form { get; set; } = new();

    /// <summary>Preview data-URL shown below the file picker. Set in BindAsync; not persisted in Hydro state.</summary>
    public string? PreviewImageSource { get; set; }

    [Transient]
    public IFormFile? ImageFile { get; set; }

    public override async Task MountAsync()
    {
        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var p = await partnerService.GetPartnerByIdAsync(Id.Value);

            if (p != null)
            {
                Form = new FormModel
                {
                    Name = p.Name,
                    LogoUrl = p.LogoUrl,
                    WebsiteLink = p.WebsiteLink
                };
                PreviewImageSource = p.LogoUrl;
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
            
            Form.LogoUrl = await imageService.UploadImageAsync("partners", $"{Guid.NewGuid()}{Form.NewImageExtension}", bytes);
        }

        var saveDto = new PartnerSaveDto(Form.Name, Form.LogoUrl, Form.WebsiteLink);

        if (Id.HasValue && Id.Value != Guid.Empty) { await partnerService.UpdatePartnerAsync(Id.Value, saveDto); }
        else { await partnerService.CreatePartnerAsync(saveDto); }

        Redirect(Url.Page("/Admin/Partner/Index"));
    }

    public class Validator : AbstractValidator<AdminPartnerEditor>
    {
        public Validator()
        {
            RuleFor(x => x.Form.Name)
                .NotEmpty().WithMessage("Partner name is required")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters");

            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Logo file is required")
                .When(x => !x.Id.HasValue || x.Id.Value == Guid.Empty);

            RuleFor(x => x.Form.WebsiteLink)
                .NotEmpty().WithMessage("Website link is required")
                .MaximumLength(1024).WithMessage("Website link cannot exceed 1024 characters");
        }
    }
}
