using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Ngo;

public class NgoModel(INgoService ngoService) : PageModel
{
    public NgoSummaryDto[] Ngos { get; set; } = [];

    public async Task OnGetAsync()
    {
        Ngos = await ngoService.GetAllNgosAsync();
    }
}
