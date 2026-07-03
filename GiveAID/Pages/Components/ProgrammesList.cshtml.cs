using Hydro;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;

namespace GiveAID.Pages.Components;

public class ProgrammesList(IProgrammeService programmeService) : HydroComponent
{
    public string SearchTerm { get; set; } = string.Empty;
    public string Cause { get; set; } = string.Empty;
    public string Ngo { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;

    public IEnumerable<ProgrammeSummaryDto> Programmes { get; set; } = Array.Empty<ProgrammeSummaryDto>();
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / 6.0);

    public override async Task MountAsync()
    {
        await LoadProgrammesAsync();
    }

    public async Task Search()
    {
        PageNumber = 1;
        await LoadProgrammesAsync();
    }

    public async Task ChangePage(int page)
    {
        PageNumber = page;
        await LoadProgrammesAsync();
    }

    public async Task ClearFilters()
    {
        SearchTerm = string.Empty;
        Cause = string.Empty;
        Ngo = string.Empty;
        PageNumber = 1;
        await LoadProgrammesAsync();
    }

    private async Task LoadProgrammesAsync()
    {
        var query = new ProgrammeQueryParameters
        {
            SearchTerm = SearchTerm,
            Cause = Cause,
            Ngo = Ngo,
            PageNumber = PageNumber,
            PageSize = 6
        };
        var result = await programmeService.GetProgrammesAsync(query);
        Programmes = result.Programmes;
        TotalCount = result.TotalCount;
    }
}
