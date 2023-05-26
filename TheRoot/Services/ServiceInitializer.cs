using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Find.Commerce;
using Headless.Features.Find;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Headless.Services
{
    public static class ServiceInitializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddSingleton<MenuFactory>();
            services.AddSingleton<ViewFactory>();
            services.AddSingleton<UserStore<ApplicationUser>>();
            services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false);

            //find content indexing
            services.AddSingleton<CatalogContentClientConventions, FindConventions>();

        }
    }
}
