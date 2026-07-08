using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Partner;

public class AdminPartnerEditor(IPartnerService partnerService) : HydroComponent
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
        var saveDto = new PartnerSaveDto(Form.Name, Form.LogoUrl, Form.Description, Form.WebsiteLink);

        if (Id.HasValue && Id.Value != Guid.Empty) { await partnerService.UpdatePartnerAsync(Id.Value, saveDto); }
        else { await partnerService.CreatePartnerAsync(saveDto); }

        Redirect(Url.Page("/Admin/Partner"));
    }
}
