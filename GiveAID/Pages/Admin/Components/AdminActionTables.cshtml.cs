using GiveAID.Data;
using GiveAID.Models;
using Hydro;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GiveAID.Pages.Admin.Components;

public class ApprovalRequest
{
    public Guid Id { get; set; }
    public string Requester { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

public class RecentDonationDto
{
    public Guid Id { get; set; }
    public string DonorName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? ProgrammeName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DonationStatus Status { get; set; }
}

public class AdminActionTables(AppDbContext context) : HydroComponent
{
    public List<RecentDonationDto> RecentDonations { get; set; } = new();
    public List<ApprovalRequest> PendingRequests { get; set; } = new();

    public string DonationDataJson { get; set; } = "[]";
    public string DonationLabelsJson { get; set; } = "[]";
    public string CampaignDataJson { get; set; } = "[]";
    public string CampaignLabelsJson { get; set; } = "[]";
    public string NgoDataJson { get; set; } = "[]";
    public string NgoLabelsJson { get; set; } = "[]";
    public string UserDataJson { get; set; } = "[]";
    public string UserLabelsJson { get; set; } = "[]";

    // Summary pills
    public decimal DonationTotal { get; set; }
    public int DonationCount { get; set; }
    public decimal DonationThisMonth { get; set; }
    public int CampaignTotal { get; set; }
    public int CampaignCauseCount { get; set; }
    public int NgoTotal { get; set; }
    public int NgoThisMonth { get; set; }
    public int UserTotal { get; set; }
    public int UserThisMonth { get; set; }

    public override async Task MountAsync()
    {
        RecentDonations = await context.Donations
            .Include(d => d.Programme)
            .Include(d => d.User)
            .OrderByDescending(d => d.CreatedAt)
            .Take(5)
            .Select(d => new RecentDonationDto
            {
                Id = d.DonationId,
                DonorName = d.User.FullName,
                Amount = d.Amount,
                ProgrammeName = d.Programme != null ? d.Programme.Name : null,
                CreatedAt = d.CreatedAt,
                Status = d.Status
            })
            .ToListAsync();

        PendingRequests = await context.UserQueries
            .Include(q => q.User)
            .Where(q => string.IsNullOrEmpty(q.ReplyText))
            .OrderByDescending(q => q.CreatedAt)
            .Take(5)
            .Select(q => new ApprovalRequest
            {
                Id = q.QueryId,
                Requester = q.User.FullName,
                Type = q.Subject
            })
            .ToListAsync();

        // 1. Donation Trends (Current Year)
        var currentYear = DateTimeOffset.UtcNow.Year;
        var yearDonations = await context.Donations
            .Where(d => d.CreatedAt.Year == currentYear && d.Status == DonationStatus.Completed)
            .Select(d => new { d.CreatedAt, d.Amount })
            .ToListAsync();

        var donationGroups = yearDonations
            .GroupBy(d => d.CreatedAt.Month)
            .OrderBy(g => g.Key)
            .ToList();

        var dLabels = new List<string>();
        var dData = new List<decimal>();
        for (int i = 1; i <= 12; i++)
        {
            dLabels.Add(new DateTime(currentYear, i, 1).ToString("MMM"));
            dData.Add(donationGroups.FirstOrDefault(g => g.Key == i)?.Sum(d => d.Amount) ?? 0m);
        }
        DonationLabelsJson = JsonSerializer.Serialize(dLabels);
        DonationDataJson = JsonSerializer.Serialize(dData);

        // Donation pills
        var thisMonth = DateTimeOffset.UtcNow.Month;
        DonationTotal = yearDonations.Sum(d => d.Amount);
        DonationCount = yearDonations.Count;
        DonationThisMonth = yearDonations
            .Where(d => d.CreatedAt.Month == thisMonth)
            .Sum(d => d.Amount);

        // 2. Campaign Distribution by Cause
        var campaigns = await context.AvailableWelfareProgrammes
            .Include(p => p.Cause)
            .Select(p => p.Cause.Name)
            .ToListAsync();

        var causeGroups = campaigns
            .GroupBy(c => c)
            .Select(g => new { Cause = g.Key, Count = g.Count() })
            .ToList();

        CampaignLabelsJson = JsonSerializer.Serialize(causeGroups.Select(g => g.Cause));
        CampaignDataJson = JsonSerializer.Serialize(causeGroups.Select(g => g.Count));

        // Campaign pills
        CampaignTotal = campaigns.Count;
        CampaignCauseCount = causeGroups.Count;

        // 3. NGO Onboarding (Last 6 Months)
        var sixMonthsAgo = DateTimeOffset.UtcNow.AddMonths(-5);
        var ngos = await context.Ngos
            .Where(n => !n.IsDeleted)
            .Select(n => n.CreatedAt)
            .ToListAsync();

        var ngoLabels = new List<string>();
        var ngoData = new List<int>();
        for (int i = 5; i >= 0; i--)
        {
            var date = DateTimeOffset.UtcNow.AddMonths(-i);
            ngoLabels.Add(date.ToString("MMM yyyy"));
            ngoData.Add(ngos.Count(n => n.Year == date.Year && n.Month == date.Month));
        }
        NgoLabelsJson = JsonSerializer.Serialize(ngoLabels);
        NgoDataJson = JsonSerializer.Serialize(ngoData);

        // NGO pills
        NgoTotal = ngos.Count;
        NgoThisMonth = ngos.Count(n => n.Year == DateTimeOffset.UtcNow.Year && n.Month == DateTimeOffset.UtcNow.Month);

        // 4. User Acquisition (Last 6 Months)
        var users = await context.Users
            .Where(u => !u.IsDeleted)
            .Select(u => u.CreatedAt)
            .ToListAsync();

        var userLabels = new List<string>();
        var userData = new List<int>();
        for (int i = 5; i >= 0; i--)
        {
            var date = DateTimeOffset.UtcNow.AddMonths(-i);
            userLabels.Add(date.ToString("MMM yyyy"));
            userData.Add(users.Count(n => n.Year == date.Year && n.Month == date.Month));
        }
        UserLabelsJson = JsonSerializer.Serialize(userLabels);
        UserDataJson = JsonSerializer.Serialize(userData);

        // User pills
        UserTotal = users.Count;
        UserThisMonth = users.Count(n => n.Year == DateTimeOffset.UtcNow.Year && n.Month == DateTimeOffset.UtcNow.Month);
    }
}
