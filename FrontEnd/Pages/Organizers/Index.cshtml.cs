using System.Text.Json;
using AutoMapper;
using DevExtreme.AspNet.Data;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Organizers;

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

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnGetReadAsync(DataSourceLoadOptionsBase set)
    {
        var pages = set.Take == 0 ? 10 : set.Take;
        var response = await _sportEventApi.Ready().GetAllOrganizersAsync(set.Skip / pages +  1, pages);

        return new JsonResult(new { totalCount = response.Meta.Pagination.Total, data = response.Data });
    }

    public async Task OnPostCreateAsync(IFormCollection collection)
    {
        string? grup = collection["values"].FirstOrDefault();
        if (!string.IsNullOrEmpty(grup))
        {
            var rdto = JsonSerializer.Deserialize<CreateUpdateOrganizer>(grup, _jOpt.JOpts());

            await _sportEventApi.Ready().CreateOrganizerAsync(rdto);
        }
    }

    public async Task<IActionResult?> OnPutEditAsync(IFormCollection collection)
    {

        string? grupId = collection["key"].FirstOrDefault();
        string? grup = collection["values"].FirstOrDefault();
        if (grup == null || grupId == null)
        {
            return BadRequest("Data Error");
        }

        var source = await _sportEventApi.Ready().GetOrganizerAsync(grupId);

        var rdto = JsonSerializer.Deserialize<CreateUpdateOrganizer>(grup, _jOpt.JOpts());
        var configs = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Organizer, CreateUpdateOrganizer>().ForMember(dest => dest.OrganizerName, act => act.MapFrom((src, dest) => dest.OrganizerName ?? src.OrganizerName)).ForMember(dest => dest.ImageLocation, act => act.MapFrom((src, dest) => dest.ImageLocation ?? src.ImageLocation));
        });
        var mappers = new Mapper(configs);
        mappers.Map(source, rdto);

        await _sportEventApi.Ready().UpdateOrganizerAsync(Convert.ToInt32(grupId), rdto);
        return null;
    }

    public async Task<IActionResult?> OnDeleteDeleteAsync(IFormCollection collection)
    {
        try
        {
            string? grupId = collection["key"].FirstOrDefault();
            if (grupId == null)
            {
                return BadRequest("Data Error");
            }

            await _sportEventApi.Ready().DeleteOrganizerAsync(Convert.ToInt32(grupId));
            return null;
        }
        catch
        {
            return BadRequest("Server Error");
        }
    }
}