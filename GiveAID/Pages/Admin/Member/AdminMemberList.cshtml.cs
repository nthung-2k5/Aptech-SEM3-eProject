using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.Member;

public class AdminMemberList(IMemberService memberService) : HydroComponent
{
    public MemberDto[] Members { get; set; } = [];
    public override async Task MountAsync() { Members = await memberService.GetAllMemberDtosAsync(null); }

    public async Task Delete(Guid id)
    {
        await memberService.DeleteMemberAsync(id);
        Members = await memberService.GetAllMemberDtosAsync(null);
    }
}
