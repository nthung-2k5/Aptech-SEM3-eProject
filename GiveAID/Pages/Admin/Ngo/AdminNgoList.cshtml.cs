using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Ngo;

public class AdminNgoList(INgoService ngoService) : HydroComponent
{
    public NgoSummaryDto[] Ngos { get; set; } = [];
    public override async Task MountAsync() { Ngos = await ngoService.GetAllNgosAsync(); }

    public async Task Delete(Guid id)
    {
        await ngoService.DeleteNgoAsync(id);
        Ngos = await ngoService.GetAllNgosAsync();
    }
}
