using System.Security.Claims;
using FrontEnd.Models;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Auth;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly IConfiguration _iConfig;
    private readonly IAuthSsoKc _authClaims;

    public LoginModel(IConfiguration iConfig, IAuthSsoKc authClaims)
    {
        _iConfig = iConfig;
        _authClaims = authClaims;
    }

    [BindProperty]
    public LoginForm? Login { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostChallenge()
    {
        try
        {
            IConfigurationSection urlCap = _iConfig.GetSection("Configs");

            if (Login == null)
                return BadRequest(
                    "Your username is not registered in our application or your username / password is wrong. Error Code: 1");

            var claimings = await _authClaims.AddClaims(Login.Email, Login.Password);
            if (claimings != null)
            {
                //ClaimsIdentity claimsIdentity = claimings.claimIdent;

                AuthenticationProperties authProperties = new()
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across  
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    RedirectUri = CookieAuthenticationDefaults.ReturnUrlParameter,
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimings.claimIdent),
                    authProperties);
            }

            return new JsonResult(new { claimings?.Pic, tok = claimings?.Tok });

        }
        catch (Exception e)
        {
            return BadRequest("Application server currently is not available. Please, try again in couple of minutes or you may contact helpdesk. Error Code: 4");
        }
    }
}