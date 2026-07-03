namespace GiveAID.Services;

using GiveAID.Services.Abstractions;
using GiveAID.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

public class ProgrammeService : IProgrammeService
{
    private static readonly List<ProgrammeDetailsDto> MockProgrammes = new()
    {
        new ProgrammeDetailsDto(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            "Education for All",
            "Education",
            "EduCare Foundation",
            "https://images.unsplash.com/photo-1509062522246-3755977927d7",
            DateTime.Now.AddDays(-10),
            DateTime.Now.AddDays(20),
            1250,
            50000m,
            25000m,
            "Providing basic education for underprivileged children in rural areas.",
            "EduCare Foundation has been working in rural education since 2010."
        ),
        new ProgrammeDetailsDto(
            Guid.Parse("22222222-2222-2222-2222-222222222222"),
            "Clean Water Initiative",
            "Health",
            "Water Aid",
            "https://images.unsplash.com/photo-1519897831810-a9a01ace64f7",
            DateTime.Now.AddDays(5),
            DateTime.Now.AddDays(35),
            300,
            20000m,
            5000m,
            "Building wells to provide clean water to drought-affected communities.",
            "Water Aid focuses on clean water and sanitation."
        ),
        new ProgrammeDetailsDto(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            "Food Security Program",
            "Hunger",
            "Food Bank",
            "https://images.unsplash.com/photo-1593113589914-07599019ddb0",
            DateTime.Now.AddDays(-30),
            DateTime.Now.AddDays(60),
            4500,
            100000m,
            85000m,
            "Distributing food supplies to vulnerable families during crisis.",
            "Food Bank is a network of local food pantries."
        ),
        new ProgrammeDetailsDto(
            Guid.Parse("44444444-4444-4444-4444-444444444444"),
            "Women Empowerment",
            "Inclusion",
            "Global Women",
            "https://images.unsplash.com/photo-1573164713988-8665fc963095",
            DateTime.Now.AddDays(15),
            DateTime.Now.AddDays(100),
            120,
            15000m,
            2000m,
            "Vocational training programs for women in developing regions.",
            "Global Women works to empower women economically."
        ),
        new ProgrammeDetailsDto(
            Guid.Parse("55555555-5555-5555-5555-555555555555"),
            "Disaster Relief",
            "Emergency",
            "Red Cross",
            "https://images.unsplash.com/photo-1469571486292-0ba58a3f068b",
            DateTime.Now,
            DateTime.Now.AddDays(14),
            8900,
            null,
            150000m,
            "Immediate relief for communities affected by natural disasters.",
            "Red Cross provides emergency assistance and disaster relief."
        )
    };

    public Task<(IEnumerable<ProgrammeSummaryDto> Programmes, int TotalCount)> GetProgrammesAsync(ProgrammeQueryParameters query)
    {
        var q = MockProgrammes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            q = q.Where(p => p.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase) || 
                             p.Description.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.Ngo))
        {
            q = q.Where(p => p.Ngo.Equals(query.Ngo, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(query.Cause))
        {
            q = q.Where(p => p.Cause.Equals(query.Cause, StringComparison.OrdinalIgnoreCase));
        }

        int totalCount = q.Count();

        var paginated = q.Skip((query.PageNumber - 1) * query.PageSize)
                         .Take(query.PageSize)
                         .Select(p => new ProgrammeSummaryDto(
                             p.Id,
                             p.Name,
                             p.Cause,
                             p.Ngo,
                             p.ImageUrl,
                             p.StartDate,
                             p.EndDate,
                             p.DonationCount,
                             p.TargetAmount,
                             p.RaisedAmount
                         ))
                         .ToList();

        return Task.FromResult(((IEnumerable<ProgrammeSummaryDto>)paginated, totalCount));
    }

    public Task<ProgrammeDetailsDto?> GetProgrammeDetailsAsync(Guid id)
    {
        var prog = MockProgrammes.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(prog);
    }
}
