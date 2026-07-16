using System.Security.Claims;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Member;

public class DonationList(IDonationService donationService, IHttpContextAccessor httpContextAccessor) : HydroComponent
{
    public UserDonationQueryParameters Query { get; set; } = new();
    
    public PagedResult<UserDonationDto> Donations { get; set; } = new([], 0, 1, 10);

    public override async Task MountAsync()
    {
        await LoadDonationsAsync();
    }

    public async Task ApplyFiltersAsync()
    {
        Query.PageNumber = 1;
        await LoadDonationsAsync();
    }

    public async Task PageChangedAsync(int page)
    {
        Query.PageNumber = page;
        await LoadDonationsAsync();
    }

    public async Task ClearFilter()
    {
        Query = new UserDonationQueryParameters();
        await ApplyFiltersAsync();
    }

    private async Task LoadDonationsAsync()
    {
        var user = httpContextAccessor.HttpContext?.User;
        string? userIdStr = user?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (Guid.TryParse(userIdStr, out var userId))
        {
            Donations = await donationService.GetDonationsByUserPagedAsync(userId, Query);
        }
    }
}
