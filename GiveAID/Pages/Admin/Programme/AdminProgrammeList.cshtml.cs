using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Programme;

public class AdminProgrammeList(IProgrammeService programmeService, INgoService ngoService, IDonationCauseService causeService) : HydroComponent
{
    // Filter / Search state
    public string SearchTerm { get; set; } = string.Empty;
    public string? NgoId { get; set; }
    public string? CauseId { get; set; }
    public string StatusFilter { get; set; } = string.Empty; // "" | "Active" | "Upcoming" | "Ended"
    public int PageNumber { get; set; } = 1;
    private const int PageSize = 10;

    // Results
    public ProgrammeDto[] Programmes { get; set; } = [];
    public int TotalCount { get; set; }
    public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    // Dropdown data
    public NgoSummaryDto[] AvailableNgos { get; set; } = [];
    public DonationCauseDto[] AvailableCauses { get; set; } = [];

    public override async Task MountAsync()
    {
        AvailableNgos = await ngoService.GetAllNgosAsync();
        AvailableCauses = await causeService.GetAllDonationCausesAsync();
        await LoadDataAsync();
    }

    public async Task Search()
    {
        PageNumber = 1;
        await LoadDataAsync();
    }

    public async Task GoToPage(int page)
    {
        PageNumber = page;
        await LoadDataAsync();
    }

    public async Task Delete(Guid id)
    {
        await programmeService.DeleteProgrammeAsync(id);
        await LoadDataAsync();
        Client.ExecuteJs("Swal.fire('Deleted!', 'The record has been deleted.', 'success');");
    }

    private async Task LoadDataAsync()
    {
        var query = new ProgrammeQueryParameters
        {
            SearchTerm = SearchTerm,
            NgoId = !string.IsNullOrEmpty(NgoId) ? Guid.Parse(NgoId) : null,
            CauseId = !string.IsNullOrEmpty(CauseId) ? Guid.Parse(CauseId) : null,
            StatusFilter = StatusFilter,
            PageNumber = PageNumber,
            PageSize = PageSize
        };

        var result = await programmeService.GetAllProgrammesPagedAsync(query);
        Programmes = result.Items;
        TotalCount = result.TotalCount;
    }
}
