using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Programmes;

public class ProgrammesList(IProgrammeService programmeService) : HydroComponent
{
    public string SearchTerm { get; set; } = string.Empty;
    public string? CauseId { get; set; }
    public string? NgoId { get; set; }
    public string SortOption { get; set; } = "startdate_desc";
    public int PageNumber { get; set; } = 1;

    public PagedResult<ProgrammeDto> Programmes { get; set; } = new();
    public int TotalPages => Programmes.TotalPages;

    public override async Task MountAsync() { await LoadProgrammesAsync(); }

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
        CauseId = null;
        NgoId = null;
        SortOption = "startdate_desc";
        PageNumber = 1;
        await LoadProgrammesAsync();
    }

    private async Task LoadProgrammesAsync()
    {
        string[] sortParts = SortOption.Split('_');
        string sortBy = sortParts.Length > 0 ? sortParts[0] : "startdate";
        bool sortDesc = sortParts.Length <= 1 || sortParts[1] == "desc";

        var query = new ProgrammeQueryParameters
        {
            SearchTerm = SearchTerm,
            CauseId = !string.IsNullOrEmpty(CauseId) ? Guid.Parse(CauseId) : null,
            NgoId = !string.IsNullOrEmpty(NgoId) ? Guid.Parse(NgoId) : null,
            SortBy = sortBy,
            SortDescending = sortDesc,
            PageNumber = PageNumber,
            PageSize = 6
        };
        Programmes = await programmeService.GetAllProgrammesPagedAsync(query);
    }
}
