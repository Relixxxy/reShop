namespace MVC;

public class AppSettings
{
    public string CatalogUrl { get; set; } = null!;
    public string BasketUrl { get; set; } = null!;
    public string OrderUrl { get; set; } = null!;
    public int SessionCookieLifetimeMinutes { get; set; }
    public string CallBackUrl { get; set; } = null!;
    public string IdentityUrl { get; set; } = null!;
}
