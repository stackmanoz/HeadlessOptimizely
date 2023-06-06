using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Find.Commerce;
using IDM.Application.Features.Find;
using IDM.Application.Services.Identity;
using IDM.Infrastructure.Services.CatalogService;
using Klarna.Common.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IDM.Application.Services;

public static class ServiceInitializer
{
    public static void InitializeServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<UserStore<ApplicationUser>>();
        services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = false);

        //find content indexing
        services.AddSingleton<CatalogContentClientConventions, FindConventions>();

        services.Configure<CheckoutConfiguration>("EU", configuration.GetSection("Klarna:Checkout:EU"));
        services.AddSingleton<IAccountManager, AccountManager>();
        services.AddSingleton<ICatalogRootService, CatalogRootService>();
        services.AddSingleton<MenuService>();
    }
}