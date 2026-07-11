using FluentValidation;
using GiveAID.Dtos;
using GiveAID.Exceptions;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Ngo;

public class AdminNgoEditor(
    INgoService ngoService,
    IPartnerService partnerService,
    IValidator<AdminNgoEditor> validator) : HydroComponent
{
    public Guid? Id { get; set; }

    public class FormModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Address { get; set; } = "";
        public string Website { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public List<Guid> SelectedPartnerIds { get; set; } = [];
    }

    public FormModel Form { get; set; } = new();
    public PartnerSummaryDto[] AvailablePartners { get; set; } = [];

    public override async Task MountAsync()
    {
        AvailablePartners = await partnerService.GetAllPartnersAsync();

        if (Id.HasValue && Id.Value != Guid.Empty)
        {
            var n = await ngoService.GetNgoByIdAsync(Id.Value);

            if (n != null)
            {
                Form = new FormModel
                {
                    Name = n.Name, Description = n.Description,
                    Address = n.Address ?? "", Website = n.Website ?? "", PhoneNumber = n.PhoneNumber ?? "",
                    SelectedPartnerIds = n.Partners.Select(p => p.Id).ToList()
                };
            }
        }
    }

    public void TogglePartner(Guid partnerId)
    {
        if (!Form.SelectedPartnerIds.Remove(partnerId)) { Form.SelectedPartnerIds.Add(partnerId); }
    }

    public async Task Save()
    {
        if (!this.Validate(validator)) { return; }

        var saveDto = new NgoSaveDto(
            Form.Name,
            Form.Description,
            Form.Address,
            Form.PhoneNumber,
            Form.Website,
            Form.SelectedPartnerIds.ToArray());

        try
        {
            if (Id.HasValue && Id.Value != Guid.Empty) { await ngoService.UpdateNgoAsync(Id.Value, saveDto); }
            else { await ngoService.CreateNgoAsync(saveDto); }

            Redirect(Url.Page("/Admin/Ngo/Index"));
        }
        catch (DuplicateException ex)
        {
            if (ex.FieldName == nameof(NgoSaveDto.Name))
            {
                ModelState.AddModelError($"Form.{nameof(Form.Name)}", "An NGO with this name already exists");
            }
            else { ModelState.AddModelError($"Form.{ex.FieldName}", ex.Message); }
        }
    }

    public class Validator : AbstractValidator<AdminNgoEditor>
    {
        public Validator()
        {
            RuleFor(x => x.Form.Name)
                .NotEmpty().WithMessage("NGO name is required")
                .MaximumLength(255).WithMessage("Name cannot exceed 255 characters");

            RuleFor(x => x.Form.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.Form.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters");

            RuleFor(x => x.Form.Website)
                .MaximumLength(1024).WithMessage("Website cannot exceed 1024 characters");

            RuleFor(x => x.Form.PhoneNumber)
                    .PhoneNumber().WithMessage("Phone number must be in E.164 format");
        }
    }
}
