using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using System.Threading.Tasks;
using System;

namespace GiveAID.Pages;

public class ProgrammeDetailsModel : PageModel
{
    private readonly IProgrammeService _programmeService;

    public ProgrammeDetailsModel(IProgrammeService programmeService)
    {
        _programmeService = programmeService;
    }

    public ProgrammeDetailsDto? Programme { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Programme = await _programmeService.GetProgrammeDetailsAsync(id);
        
        if (Programme == null)
        {
            return NotFound();
        }
        
        return Page();
    }
}
