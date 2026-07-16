using GiveAID.Data;
using Hydro;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Pages.Components;

public class HeroSlider(AppDbContext dbContext) : HydroComponent
{
    // Placeholders for your backend image URLs. 
    // You can later map these to your database or CMS.
    public List<string> Slides { get; set; } = new List<string>
    {
        "https://images.unsplash.com/photo-1488521787991-ed7bbaae773c?w=1600&h=900&fit=crop&auto=format",
        "https://images.unsplash.com/photo-1532629345422-7515f3d16bb6?w=1600&h=900&fit=crop&auto=format",
        "https://images.unsplash.com/photo-1469571486292-0ba58a3f068b?w=1600&h=900&fit=crop&auto=format"
    };

    public int TotalProgrammes { get; set; }
    public int TotalPartners { get; set; }
    public decimal TotalRaised { get; set; }
    public int TotalDonations { get; set; }

    public override async Task MountAsync()
    {
        TotalProgrammes = await dbContext.ActiveWelfareProgrammes.CountAsync();
        TotalPartners = await dbContext.ActiveNgos.CountAsync() + await dbContext.CorporatePartners.CountAsync();
        TotalRaised = await dbContext.ValidDonations.SumAsync(d => d.Amount);
        TotalDonations = await dbContext.ValidDonations.CountAsync();
    }
}
