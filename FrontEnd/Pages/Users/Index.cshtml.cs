using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using DevExtreme.AspNet.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Users;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ISportEventApi _sportEventApi;
    private readonly IJsonOptions _jOpt;

    public IndexModel(IJsonOptions jOpt, ISportEventApi sportEventApi)
    {
        _jOpt = jOpt;
        _sportEventApi = sportEventApi;
    }

    [BindProperty]
    public User? Users { get; set; }

    public async Task OnGet() => Users = await _sportEventApi.Ready().GetUserAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

    public async Task OnPostCreateAsync(CreateUser user) => await _sportEventApi.Ready().CreateUserAsync(user);

    public async Task OnPutEditAsync(int uId, UpdateUser user) => await _sportEventApi.Ready().UpdateUserAsync(uId, user);

    public async Task OnDeleteDeleteAsync(int uId) => await _sportEventApi.Ready().DeleteUserAsync(uId);
}