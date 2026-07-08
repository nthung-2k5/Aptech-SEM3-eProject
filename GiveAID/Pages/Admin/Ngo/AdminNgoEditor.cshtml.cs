using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;
using Microsoft.AspNetCore.Mvc;

namespace GiveAID.Pages.Admin.Ngo;

public class AdminNgoEditor(INgoService ngoService, IPartnerService partnerService) : HydroComponent
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
                    SelectedPartnerIds = n.Partners.Select(p => p.PartnerId).ToList()
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
        var saveDto = new NgoSaveDto(
            Form.Name,
            Form.Description,
            Form.Address,
            Form.PhoneNumber,
            Form.Website,
            Form.SelectedPartnerIds.ToArray());

        if (Id.HasValue && Id.Value != Guid.Empty) { await ngoService.UpdateNgoAsync(Id.Value, saveDto); }
        else { await ngoService.CreateNgoAsync(saveDto); }

        Redirect(Url.Page("/Admin/Ngo"));
    }
}
