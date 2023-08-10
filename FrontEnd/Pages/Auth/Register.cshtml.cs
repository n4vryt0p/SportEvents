using FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Auth;

[ValidateAntiForgeryToken]
public class RegisterModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _iConfig;

    public RegisterModel(IHttpClientFactory httpClientFactory, IConfiguration iConfig, LoginForm? regist)
    {
        _httpClientFactory = httpClientFactory;
        _iConfig = iConfig;
        Regist = regist;
    }

    [BindProperty]
    public LoginForm? Regist { get; set; }

    public void OnGet()
    {
    }
}