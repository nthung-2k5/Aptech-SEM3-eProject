using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Programmes;

public class ProgrammesList(IProgrammeService programmeService) : HydroComponent
{
    public string SearchTerm { get; set; } = string.Empty;
    public string? CauseId { get; set; }
    public string? NgoId { get; set; }
    public DateTime? DateFilter { get; set; }
    public int PageNumber { get; set; } = 1;

    public ProgrammeDto[] Programmes { get; set; } = [];
    public int TotalPages => (int)Math.Ceiling(Programmes.Length / 6.0);

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
        DateFilter = null;
        PageNumber = 1;
        await LoadProgrammesAsync();
    }

    private async Task LoadProgrammesAsync()
    {
        var query = new ProgrammeQueryParameters
        {
            SearchTerm = SearchTerm,
            CauseId = !string.IsNullOrEmpty(CauseId) ? Guid.Parse(CauseId) : null,
            NgoId = !string.IsNullOrEmpty(NgoId) ? Guid.Parse(NgoId) : null,
            DateFilter = DateFilter,
            PageNumber = PageNumber,
            PageSize = 6
        };
        Programmes = await programmeService.GetAllProgrammesAsync(query);
    }
}
