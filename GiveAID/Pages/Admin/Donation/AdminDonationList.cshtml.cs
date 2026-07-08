using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Donation;

public class AdminDonationList(IDonationService donationService) : HydroComponent
{
    public DonationDto[] Donations { get; set; } = [];
    public override async Task MountAsync() { Donations = await donationService.GetAllDonationsAsync(); }

    public async Task Void(Guid id)
    {
        await donationService.VoidDonationAsync(id);
        Donations = await donationService.GetAllDonationsAsync();
    }
}
