using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Admin;

public class IndexModel(
    IDonationService donationService,
    IMemberService memberService,
    INgoService ngoService,
    IUserQueryService queryService
) : PageModel
{
    public int TotalDonations { get; set; }
    public int TotalMembers { get; set; }
    public int TotalNgos { get; set; }
    public int OpenQueriesCount { get; set; }

    public List<DonationDto> LatestDonations { get; set; } = [];
    public List<UserQueryDto> LatestUnansweredQueries { get; set; } = [];

    public async Task OnGetAsync()
    {
        var donations = await donationService.GetAllDonationsAsync();
        TotalDonations = donations.Length;
        LatestDonations = donations.OrderByDescending(d => d.DonationDate).Take(5).ToList();

        var members = await memberService.GetAllMembersAsync();
        TotalMembers = members.Length;

        var ngos = await ngoService.GetAllNgosAsync();
        TotalNgos = ngos.Length;

        var unanswered = await queryService.GetUnansweredQueriesAsync();
        OpenQueriesCount = unanswered.Length;
        LatestUnansweredQueries = unanswered.OrderByDescending(q => q.CreatedAt).Take(5).ToList();
    }
}
