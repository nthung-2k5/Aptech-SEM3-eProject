using GiveAID.Dtos;
using GiveAID.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages;

public class IndexModel(IDonationCauseService causeService) : PageModel
{
    public DonationCauseDto[] Causes { get; set; } = [];

    public async Task OnGetAsync()
    {
        Causes = await causeService.GetAllDonationCausesAsync();
    }
}
