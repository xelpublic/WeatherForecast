using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wf.Frontend.Pages.Account;

public class LoginModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; set; } = "/";

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return LocalRedirect(ReturnUrl);
        }
    
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = ReturnUrl
        }, "oidc");
    }
}