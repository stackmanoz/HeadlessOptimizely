using EPiServer.Commerce.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using IDM.Application.Features.CMS.Pages.HomePage.Services;
using IDM.Application.Features.Commerce.Checkout.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace IDM.Application.Features.InitializationModules
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class SiteInitialization : IConfigurableModule
    {
        private IServiceCollection _services;

        public void Initialize(InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(false);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
            _services.AddSingleton<IServiceCollection, ServiceCollection>();
            _services.AddSingleton<IHomePageModelFactory, HomePageFactory>();
            _services.AddSingleton<ICustomPaymentService, CustomPaymentService>();
            _services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            ServiceCollectionServiceExtensions.AddScoped(_services, x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }
    }
}
