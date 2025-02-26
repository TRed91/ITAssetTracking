namespace ITAssetTracking.MVC;

public class AppConfig
{
    private readonly IConfiguration _config;

    public AppConfig(IConfiguration configuration)
    {
        _config = configuration;
    }
}