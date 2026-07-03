using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GiveAID.Pages;

public class ProgrammesModel : PageModel
{
    private readonly IProgrammeService _programmeService;

    public ProgrammesModel(IProgrammeService programmeService)
    {
        _programmeService = programmeService;
    }

    [BindProperty(SupportsGet = true)]
    public ProgrammeQueryParameters Query { get; set; } = new ProgrammeQueryParameters { PageSize = 6 };

    public IEnumerable<ProgrammeSummaryDto> Programmes { get; set; } = Array.Empty<ProgrammeSummaryDto>();
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)(Query.PageSize > 0 ? Query.PageSize : 6));

    public async Task OnGetAsync()
    {
        if (Query.PageSize <= 0) Query.PageSize = 6;
        
        var result = await _programmeService.GetProgrammesAsync(Query);
        Programmes = result.Programmes;
        TotalCount = result.TotalCount;
    }
}
