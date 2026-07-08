using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Partner;

public class AdminPartnerList(IPartnerService partnerService) : HydroComponent
{
    public PartnerDto[] Partners { get; set; } = [];

    public override async Task MountAsync() { Partners = await partnerService.GetAllPartnerDtosAsync(); }

    public async Task Delete(Guid id)
    {
        await partnerService.DeletePartnerAsync(id);
        Partners = await partnerService.GetAllPartnerDtosAsync();
    }
}
