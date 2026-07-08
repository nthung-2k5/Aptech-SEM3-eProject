using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Donation;

public class AdminDonationList(IDonationService donationService, IMemberService memberService) : HydroComponent
{
    public DonationDto[] Donations { get; set; } = [];
    public override async Task MountAsync() { Donations = await donationService.GetAllDonationsAsync(); }

    public async Task Void(Guid id)
    {
        await donationService.VoidDonationAsync(id);
        Donations = await donationService.GetAllDonationsAsync();
    }
    
    public bool OpenModal { get; set; }
    public MemberDto? SelectedMember { get; set; }
    
    public async Task ShowMemberInfo(Guid memberId)
    {
        var member = await memberService.GetMemberByIdAsync(memberId);

        OpenModal = true;
        if (member != null) { SelectedMember = member; }
    }

    public void CloseModal()
    {
        OpenModal = false;
    }
}
