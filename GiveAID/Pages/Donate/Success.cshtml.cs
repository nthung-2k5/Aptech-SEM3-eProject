using GiveAID.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Donate;

[Authorize(Roles = nameof(UserRole.Member))]
public class SuccessModel : PageModel
{
    public Guid TransactionId { get; set; }

    public void OnGet(Guid transactionId) { TransactionId = transactionId; }
}
