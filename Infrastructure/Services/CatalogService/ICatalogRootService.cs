using EPiServer.Commerce.Catalog.ContentTypes;

namespace IDM.Infrastructure.Services.CatalogService
{
    public interface ICatalogRootService
    {
        CatalogContentBase GetCatalogRoot();
    }
}
