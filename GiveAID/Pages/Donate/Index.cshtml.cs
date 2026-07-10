using System.Security.Claims;
using GiveAID.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Donate;

[Authorize(Roles = nameof(UserRole.Member))]
public class IndexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid? NgoId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid? ProgrammeId { get; set; }
    
    [BindNever]
    public Guid UserId { get; set; }

    public IActionResult OnGet()
    {
        UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        return Page();
    }
}
