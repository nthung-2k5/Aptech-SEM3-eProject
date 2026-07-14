using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Exceptions;
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
    }

    public FormModel Form { get; set; } = new();

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
            }
        }
    }

    public override async Task BindAsync(PropertyPath property, object value)
    {
        if (property.Name == nameof(ImageFile))
        {
            var image = (IFormFile)value;
            using var ms = new MemoryStream();
            await image.CopyToAsync(ms);
            
            var extension = Path.GetExtension(image.FileName);
            
            if (Id.HasValue && Id.Value != Guid.Empty && Form.LogoUrl.Contains("127.0.0.1:9000"))
            {
                await imageService.DeleteImageAsync(new Uri(Form.LogoUrl));
            }
            
            Form.LogoUrl = await imageService.UploadImageAsync(
                "partners",
                $"{Guid.NewGuid()}{extension}",
                ms.ToArray());
        }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        var saveDto = new PartnerSaveDto(Form.Name, Form.LogoUrl, Form.WebsiteLink);

        try
        {
            if (Id.HasValue && Id.Value != Guid.Empty) { await partnerService.UpdatePartnerAsync(Id.Value, saveDto); }
            else { await partnerService.CreatePartnerAsync(saveDto); }
        }
        catch (DuplicateException)
        {
            ModelState.AddModelError("Form.Name", "A partner with this name already exists.");
            return;
        }

        Redirect(Url.Page("/Admin/Partner/Index"));
    }

    public class Validator : AbstractValidator<AdminPartnerEditor>
    {
        public Validator()
        {
            RuleFor(x => x.Form.Name)
                .NotEmpty().WithMessage("Partner name is required")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters");

            RuleFor(x => x.Form.LogoUrl)
                .NotEmpty().WithMessage("Logo file is required")
                .OverridePropertyName(nameof(ImageFile))
                .When(x => !x.Id.HasValue || x.Id.Value == Guid.Empty);

            RuleFor(x => x.Form.WebsiteLink)
                .NotEmpty().WithMessage("Website link is required")
                .MaximumLength(1024).WithMessage("Website link cannot exceed 1024 characters");
        }
    }
}
