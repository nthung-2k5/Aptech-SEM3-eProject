using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Admin.Programme;

public class EditorModel : PageModel
{
    [FromRoute]
    public Guid? Id { get; set; }

    public void OnGet() { }
}
