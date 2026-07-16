using Hydro;
using GiveAID.Data;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Pages.Components;

public class PartnerCarousel(AppDbContext dbContext) : HydroComponent
{
    public List<PartnerDto> Partners { get; set; } = [];

    public override async Task MountAsync()
    {
        Partners = await dbContext.CorporatePartners
            .Take(10)
            .Select(p => new PartnerDto
            {
                Name = p.Name,
                LogoUrl = p.LogoUrl,
                WebsiteLink = p.WebsiteLink
            })
            .ToListAsync();
    }

    public class PartnerDto
    {
        public string Name { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string WebsiteLink { get; set; } = string.Empty;
    }
}
