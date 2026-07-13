using GiveAID.Data;
using Hydro;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Pages.Admin.Components;

public class DashboardMetrics(AppDbContext context) : HydroComponent
{
    public decimal TotalFundsRaised { get; set; }
    public int ActiveProgrammes { get; set; }
    public int PendingApprovals { get; set; }
    public int TotalPartners { get; set; }

    public override async Task MountAsync()
    {
        TotalFundsRaised = await context.ValidDonations.SumAsync(d => d.Amount);
        ActiveProgrammes = await context.ActiveWelfareProgrammes.CountAsync();
        
        // Treat unanswered user queries as pending approvals/requests
        PendingApprovals = await context.UserQueries.CountAsync(q => string.IsNullOrEmpty(q.ReplyText));
        
        TotalPartners = await context.CorporatePartners.CountAsync();
    }
}
