using System.Linq.Expressions;
using GiveAID.Dtos;
using Hydro;

namespace GiveAID.Pages.Admin.Components;

public class UserInfoModal: HydroView
{
    public bool IsOpen { get; set; }
    public MemberDto? Member { get; set; }
    public required Expression Close { get; set; }
}
