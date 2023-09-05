using EPiServer.Commerce.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using IDM.Application.Features.CMS.Pages.HomePage.Services;
using IDM.Application.Features.Commerce.Checkout.Services;
using IDM.Application.Services;
using IDM.Application.Services.CatalogMedia;
using IDM.Application.Services.Identity;
using IDM.Application.Services.Product;
using IDM.Application.Services.Variant;
using IDM.Application.Services.WebCategoryService;
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
            _services.AddSingleton<IWebCategoryService, WebCategoryService>();
            _services.AddSingleton<IAccountManager, AccountManager>();
            _services.AddSingleton<MenuService>();
            _services.AddSingleton<CustomerService>();
            _services.AddSingleton<IProductService, ProductService>();
            _services.AddSingleton<ICatalogMediaService, CatalogMediaService>();
            _services.AddSingleton<IVariantService, VariantService>();

            ServiceCollectionServiceExtensions.AddScoped(_services, x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
        }
    }
}
