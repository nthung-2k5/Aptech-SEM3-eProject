using GiveAID.Data;
using GiveAID.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiveAID.Pages.Donate;

[Authorize(Roles = nameof(UserRole.Member))]
public class SuccessModel(AppDbContext dbContext) : PageModel
{
    public Guid TransactionId { get; set; }
    public Donation? Donation { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid transactionId) 
    { 
        TransactionId = transactionId;
        
        Donation = await dbContext.Donations
            .Include(d => d.Ngo)
            .Include(d => d.Programme)
            .Include(d => d.Cause)
            .FirstOrDefaultAsync(d => d.TransactionId == transactionId);
            
        if (Donation == null)
        {
            return NotFound();
        }
        
        return Page();
    }
}
