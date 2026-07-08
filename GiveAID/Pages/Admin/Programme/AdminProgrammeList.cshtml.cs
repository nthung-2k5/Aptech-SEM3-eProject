using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Programme;

public class AdminProgrammeList(IProgrammeService programmeService) : HydroComponent
{
    public ProgrammeDto[] Programmes { get; set; } = [];

    public override async Task MountAsync()
    {
        Programmes = await programmeService.GetAllProgrammesAsync(null);
    }

    public async Task Delete(Guid id)
    {
        await programmeService.DeleteProgrammeAsync(id);
        Programmes = await programmeService.GetAllProgrammesAsync(null);
    }
}
