using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Partner;

public class AdminPartnerList(IPartnerService partnerService) : HydroComponent
{
    // Filter / Search state
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    private const int PageSize = 10;

    // Results
    public PartnerDto[] Partners { get; set; } = [];
    public int TotalCount { get; set; }
    public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public override async Task MountAsync() { await LoadDataAsync(); }

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
        await partnerService.DeletePartnerAsync(id);
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var query = new PartnerQueryParameters
        {
            SearchTerm = SearchTerm,
            PageNumber = PageNumber,
            PageSize = PageSize
        };

        var result = await partnerService.GetPartnersPagedAsync(query);
        Partners = result.Items;
        TotalCount = result.TotalCount;
    }
}
