using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.AboutUs;

public class AboutUsView(IAboutUsSubpageService aboutUsService) : HydroComponent
{
    public string? Slug { get; set; }

    public AboutUsSubpageSummaryDto[] Subpages { get; set; } = [];
    public AboutUsSubpageDto? CurrentPage { get; set; }

    public override async Task MountAsync()
    {
        Subpages = await aboutUsService.ListSubpagesAsync();

        if (Subpages.Length > 0)
        {
            if (string.IsNullOrEmpty(Slug)) 
            { 
                CurrentPage = await aboutUsService.GetBySlugAsync(Subpages[0].Slug); 
            }
            else
            {
                CurrentPage = await aboutUsService.GetBySlugAsync(Slug) ??
                              await aboutUsService.GetBySlugAsync(Subpages[0].Slug);
            }
            Slug = CurrentPage?.Slug;
        }
    }

    public async Task Delete(string slug)
    {
        await aboutUsService.DeleteSubpageAsync(slug);
        
        if (Slug == slug)
        {
            Slug = null;
        }
        await MountAsync();
        Client.ExecuteJs("Swal.fire('Deleted!', 'The subpage has been deleted.', 'success');");
    }
}
