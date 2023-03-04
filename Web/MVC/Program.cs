using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MVC.Services;
using MVC.Services.Interfaces;
using MVC.ViewModels;

var configuration = GetConfiguration();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.AddConfiguration();

var identityUrl = configuration.GetValue<string>("IdentityUrl");
var callBackUrl = configuration.GetValue<string>("CallBackUrl");
var redirectUrl = configuration.GetValue<string>("RedirectUri");
var sessionCookieLifetime = configuration.GetValue("SessionCookieLifetimeMinutes", 60);

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime))
    .AddOpenIdConnect(options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = identityUrl;
        options.Events.OnRedirectToIdentityProvider = async n =>
        {
            n.ProtocolMessage.RedirectUri = redirectUrl;
            await Task.FromResult(0);
        };
        options.SignedOutRedirectUri = callBackUrl!;
        options.ClientId = "mvc_pkce";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;
        options.UsePkce = true;
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("mvc");
    });

builder.Services.Configure<AppSettings>(configuration);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IIdentityParser<ApplicationUserVM>, IdentityParser>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute("defaultError", "{controller=Error}/{action=Error}");
app.MapControllers();

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}