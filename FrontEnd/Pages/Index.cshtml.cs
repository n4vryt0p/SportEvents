using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages;

[Authorize]
public class IndexModel : PageModel
{
    public void OnGet()
    {
    }
}