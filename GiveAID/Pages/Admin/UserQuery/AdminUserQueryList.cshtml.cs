using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.UserQuery;

public class AdminUserQueryList(IUserQueryService queryService) : HydroComponent
{
    public UserQueryDto[] Queries { get; set; } = [];
    public string ReplyText { get; set; } = "";
    public override async Task MountAsync() { Queries = await queryService.GetAllQueriesAsync(); }

    public async Task Reply(Guid id)
    {
        if (!string.IsNullOrWhiteSpace(ReplyText))
        {
            await queryService.ReplyQueryAsync(id, ReplyText);
            ReplyText = "";
            Queries = await queryService.GetAllQueriesAsync();
        }
    }
}
