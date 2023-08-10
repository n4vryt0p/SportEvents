namespace FrontEnd.Services;

public interface ISportEventApi
{
    SportEvents Ready();
}


public class SportEventApi : ISportEventApi
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _iConfig;

    public SportEventApi(IHttpClientFactory httpClientFactory, IConfiguration iConfig)
    {
        _httpClientFactory = httpClientFactory;
        _iConfig = iConfig;
    }

    public SportEvents Ready()
    {
        var baseurl = _iConfig.GetSection("Configs")["BackEndApi"];
        var httpClient = _httpClientFactory.CreateClient("BaseClient");
        return new SportEvents(baseurl, httpClient);
    }
}