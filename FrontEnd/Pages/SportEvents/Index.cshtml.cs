using System.Text.Json;
using AutoMapper;
using FrontEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.SportEvents;

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
    public ICollection<GroupDdl>? OrganizeDdl { get; set; }

    public async Task OnGet()
    {
        var fee = await _sportEventApi.Ready().GetAllOrganizersAsync(0,0);
        if (fee != null)
        {
            OrganizeDdl = fee.Data.Select(src => new GroupDdl
            {
                Id = src.Id,
                Text = src.OrganizerName
            }).ToList();
        }
    }

    public async Task<IActionResult> OnGetReadAsync(GridServerSide set)
    {
        var pages = set.Take == 0 ? 10 : set.Take;
        var response = await _sportEventApi.Ready().GetAllSportEventsAsync(set.Skip / pages + 1, pages, null);

        return new JsonResult(new { totalCount = response.Meta.Pagination.Total, data = response.Data });
    }
    public async Task OnPostCreateAsync(IFormCollection collection)
    {
        string? grup = collection["values"].FirstOrDefault();
        if (!string.IsNullOrEmpty(grup))
        {
            var rdto = JsonSerializer.Deserialize<UpdateSportEvent>(grup, _jOpt.JOpts());

            await _sportEventApi.Ready().CreateSportEventAsync(rdto);
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

        var source = await _sportEventApi.Ready().GetSportEventAsync(grupId);

        var rdto = JsonSerializer.Deserialize<UpdateSportEvent>(grup, _jOpt.JOpts());
        var configs = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SportEvent, UpdateSportEvent>()
                .ForMember(dest => dest.EventDate, act => act.MapFrom((src, dest) => dest.EventDate.Year < 1000 ? src.EventDate : dest.EventDate))
                .ForMember(dest => dest.EventName, act => act.MapFrom((src, dest) => dest.EventName ?? src.EventName))
                .ForMember(dest => dest.EventType, act => act.MapFrom((src, dest) => dest.EventType ?? src.EventType))
                .ForMember(dest => dest.OrganizerId, act => act.MapFrom((src, dest) => dest.OrganizerId < 1 ? src.OrganizerId : dest.OrganizerId));
        });
        var mappers = new Mapper(configs);
        mappers.Map(source, rdto);

        await _sportEventApi.Ready().UpdateSportEventAsync(Convert.ToInt32(grupId), rdto);
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

            await _sportEventApi.Ready().DeleteSportEventAsync(Convert.ToInt32(grupId));
            return null;
        }
        catch
        {
            return BadRequest("Server Error");
        }
    }



    //public async Task<IActionResult?> OnGetDetailsAsync(long uId)
    //{
    //    try
    //    {
    //        if (uId < 0) return null;
    //        var response = await _manageEngineApi.Ready().NewsDetailsAsync(uId);

    //        return new JsonResult(response);
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //}
}