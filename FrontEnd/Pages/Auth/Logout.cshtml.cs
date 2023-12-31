using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Auth;

[ValidateAntiForgeryToken]
public class LogoutModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }
    //public string? DenyOnly { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var cookies = HttpContext.Request.Cookies;
        if (cookies.Count > 0)
        {
            var siteCookies = cookies.Where(c => c.Key.Contains(".AspNetCore") || c.Key.Contains("Microsoft.Authentication") || c.Key.Contains("LPSIPM") || c.Key.Contains("ASP.NET")).ToArray();

            if (siteCookies.Any())
            {
                foreach (var cookie in siteCookies)
                {
                    Response.Cookies.Delete(cookie.Key);
                }
            }
        }
        
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
        

        return RedirectToPage(string.IsNullOrEmpty(ReturnUrl) ? "/auth/login" : $"/auth/login?ReturnUrl={ReturnUrl}");
    }
}