using GiveAID.Dtos;
using GiveAID.Models;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Donation;

public class AdminDonationList(IDonationService donationService, IMemberService memberService) : HydroComponent
{
    // Filter / Search state
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public string StatusFilter { get; set; } = string.Empty; // "" | "Completed" | "Void"
    public int PageNumber { get; set; } = 1;
    private const int PageSize = 10;

    // Results
    public DonationDto[] Donations { get; set; } = [];
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

    public async Task Void(Guid id)
    {
        await donationService.VoidDonationAsync(id);
        await LoadDataAsync();
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
        var query = new DonationQueryParameters
        {
            DateFrom = !string.IsNullOrEmpty(DateFrom) ? DateOnly.Parse(DateFrom) : null,
            DateTo   = !string.IsNullOrEmpty(DateTo)   ? DateOnly.Parse(DateTo)   : null,
            Status = StatusFilter switch
            {
                "Completed" => DonationStatus.Completed,
                "Void" => DonationStatus.Void,
                _ => null
            },
            PageNumber = PageNumber,
            PageSize = PageSize
        };

        var result = await donationService.GetDonationsPagedAsync(query);
        Donations = result.Items;
        TotalCount = result.TotalCount;
    }
}
