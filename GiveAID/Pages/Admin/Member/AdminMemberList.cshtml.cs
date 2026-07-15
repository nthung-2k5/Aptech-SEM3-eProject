using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Member;

public class AdminMemberList(IMemberService memberService) : HydroComponent
{
    // Filter / Search state
    public string SearchTerm { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    private const int PageSize = 10;

    // Results
    public MemberDto[] Members { get; set; } = [];
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
        await memberService.DeleteMemberAsync(id);
        await LoadDataAsync();
        Client.ExecuteJs("Swal.fire('Deleted!', 'The record has been deleted.', 'success');");
    }

    public bool OpenModal { get; set; }
    public MemberDto? SelectedMember { get; set; }

    public async Task ShowMemberInfo(Guid memberId)
    {
        var member = await memberService.GetMemberByIdAsync(memberId);
        OpenModal = true;
        if (member != null) { SelectedMember = member; }
    }

    public void CloseModal() { OpenModal = false; }

    private async Task LoadDataAsync()
    {
        var query = new MemberQueryParameters
        {
            SearchTerm = SearchTerm,
            PageNumber = PageNumber,
            PageSize = PageSize
        };

        var result = await memberService.GetMembersPagedAsync(query);
        Members = result.Items;
        TotalCount = result.TotalCount;
    }
}
