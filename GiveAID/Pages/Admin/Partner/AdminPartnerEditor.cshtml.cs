using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Partner;

public class AdminPartnerEditor(
    IPartnerService partnerService,
    IValidator<AdminPartnerEditor> validator) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
        public string LogoUrl { get; set; } = "";
        public string Description { get; set; } = "";
        public string WebsiteLink { get; set; } = "";
    }

    public FormModel Form { get; set; } = new();

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
                    Description = p.Description,
                    WebsiteLink = p.WebsiteLink
                };
            }
        }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        var saveDto = new PartnerSaveDto(Form.Name, Form.LogoUrl, Form.Description, Form.WebsiteLink);

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

            RuleFor(x => x.Form.LogoUrl)
                .NotEmpty().WithMessage("Logo URL is required")
                .MaximumLength(1024).WithMessage("Logo URL cannot exceed 1024 characters");

            RuleFor(x => x.Form.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.Form.WebsiteLink)
                .NotEmpty().WithMessage("Website link is required")
                .MaximumLength(1024).WithMessage("Website link cannot exceed 1024 characters");
        }
    }
}
