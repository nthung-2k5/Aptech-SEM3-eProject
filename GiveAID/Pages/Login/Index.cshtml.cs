using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiveAID.Pages.Login;

public class LoginModel : PageModel
{
    public void Logout()
    {
        HttpContext.Response.Cookies.Delete("jwt_token");
        Redirect("/Index");
    }
    public void OnGet()
    {
    }
}
