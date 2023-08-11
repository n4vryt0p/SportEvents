using AutoMapper;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Auth;

[ValidateAntiForgeryToken]
public class RegisterModel : PageModel
{
    private readonly ISportEventApi _sportEventApi;

    public RegisterModel(ISportEventApi sportEventApi)
    {
        _sportEventApi = sportEventApi;
    }

    [BindProperty]
    public CreateUser? Regist { get; set; }

    public void OnGet()
    {
    }

    public async void OnPost(IFormCollection ddd)
    {
        var configs = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IFormCollection, CreateUser>();
        });
        var mappers = new Mapper(configs);
        //var Regist = new CreateUser();
        mappers.Map(ddd, Regist);

        await _sportEventApi.Ready().CreateUserAsync(Regist);
    }

}