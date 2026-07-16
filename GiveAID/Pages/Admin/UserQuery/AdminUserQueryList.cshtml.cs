using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Hydro;

namespace GiveAID.Pages.Admin.UserQuery;

public class AdminUserQueryList(IUserQueryService queryService) : HydroComponent
{
    public UserQueryDto[] Queries { get; set; } = [];
    public Guid? SelectedId { get; set; }
    public bool ShowOnlyUnanswered { get; set; } = true;
    public string ReplyText { get; set; } = "";
    public DateTime? FilterDate { get; set; }

    public UserQueryDto? Selected => Queries.FirstOrDefault(q => q.Id == SelectedId);

    public override async Task MountAsync()
    {
        await LoadQueries();
    }

    private async Task LoadQueries()
    {
        var queries = ShowOnlyUnanswered
            ? await queryService.GetUnansweredQueriesAsync()
            : await queryService.GetAllQueriesAsync();

        if (FilterDate.HasValue)
        {
            queries = queries.Where(q => q.CreatedAt.Date == FilterDate.Value.Date).ToArray();
        }

        Queries = queries;
    }

    public async Task ToggleFilter()
    {
        ShowOnlyUnanswered = !ShowOnlyUnanswered;
        SelectedId = null;
        ReplyText = "";
        await LoadQueries();
    }

    public async Task Filter()
    {
        SelectedId = null;
        ReplyText = "";
        await LoadQueries();
    }

    public void SelectQuery(Guid id)
    {
        SelectedId = id;
        ReplyText = "";
    }

    public async Task Reply()
    {
        if (SelectedId.HasValue && !string.IsNullOrWhiteSpace(ReplyText))
        {
            await queryService.ReplyQueryAsync(SelectedId.Value, ReplyText);
            ReplyText = "";
            await LoadQueries();
            Client.ExecuteJs("Swal.fire('Success!', 'Reply sent successfully.', 'success');");
        }
    }
}
