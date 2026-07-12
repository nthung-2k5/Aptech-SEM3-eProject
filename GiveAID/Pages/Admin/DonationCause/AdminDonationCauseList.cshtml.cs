using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.DonationCause;

public class AdminDonationCauseList(IDonationCauseService donationCauseService) : HydroComponent
{
    // Filter / Search state
    public string SearchTerm { get; set; } = string.Empty;

    // Results
    public DonationCauseDto[] Causes { get; set; } = [];
    public int TotalCount { get; set; }

    public override async Task MountAsync() { await LoadDataAsync(); }

    public async Task Search()
    {
        await LoadDataAsync();
    }

    public async Task Delete(Guid id)
    {
        await donationCauseService.DeleteDonationCauseAsync(id);
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var allCauses = await donationCauseService.GetAllDonationCausesAsync();
        
        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            Causes = allCauses.Where(c => c.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToArray();
        }
        else
        {
            Causes = allCauses;
        }
        
        TotalCount = Causes.Length;
    }
}
