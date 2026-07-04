using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Programmes;

public class ProgrammeDetailsModel(IProgrammeService programmeService) : PageModel
{
    public ProgrammeDto? Programme { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        Programme = await programmeService.GetProgrammeByIdAsync(id);

        if (Programme == null) { return NotFound(); }

        return Page();
    }
}
