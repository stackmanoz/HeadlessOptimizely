using EPiServer.Commerce.Routing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace Headless.Features.InitializationModules
{
    [ModuleDependency(typeof(global::EPiServer.Commerce.Initialization.InitializationModule))]
    public class RegisterCommerceRouting : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(false);
        }

        public void Uninitialize(InitializationEngine context)
        {
        }
    }
}