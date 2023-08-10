namespace FrontEnd.Services;

public interface IManageEngineApi
{
    ManageEngine Ready();
}

public class ManageEngineApi : IManageEngineApi
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _iConfig;

    public ManageEngineApi(IHttpClientFactory httpClientFactory, IConfiguration iConfig)
    {
        _httpClientFactory = httpClientFactory;
        _iConfig = iConfig;
    }

    public ManageEngine Ready()
    {
        var baseurl = _iConfig.GetSection("Configs")["BackEndApi"];
        var httpClient = _httpClientFactory.CreateClient("BaseClient");
        return new ManageEngine(baseurl, httpClient);
    }
}