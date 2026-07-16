using Hydro;
using GiveAID.Data;
using GiveAID.Models;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Pages.Components;

public class ActiveProgrammes(AppDbContext dbContext) : HydroComponent
{
    public List<ProgrammeDto> Programmes { get; set; } = new List<ProgrammeDto>();

    public override async Task MountAsync()
    {
        Programmes = await dbContext.AvailableWelfareProgrammes
            .OrderByDescending(p => p.CreatedAt)
            .Take(10)
            .Select(p => new ProgrammeDto
            {
                Id = p.ProgrammeId,
                Title = p.Name,
                OrganizerName = p.Ngo.Name,
                Category = p.Cause.Name,
                ImageUrl = string.IsNullOrEmpty(p.ImageUrl) ? "https://images.unsplash.com/photo-1488521787991-ed7bbaae773c" : p.ImageUrl,
                CurrentAmount = p.Donations.Where(d => d.Status == DonationStatus.Completed).Sum(d => (decimal?)d.Amount) ?? 0,
                TargetAmount = p.MaxDonation ?? 0,
                DonationCount = p.Donations.Count(d => d.Status == DonationStatus.Completed)
            })
            .ToListAsync();
    }

    public class ProgrammeDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string OrganizerName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal CurrentAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public int DonationCount { get; set; }

        public int ProgressPercentage => TargetAmount > 0
            ? (int)Math.Min(100, (CurrentAmount / TargetAmount) * 100)
            : 0;
    }
}
