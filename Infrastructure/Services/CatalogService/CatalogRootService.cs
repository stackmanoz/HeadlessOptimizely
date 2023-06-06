using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using Mediachase.Commerce.Catalog;

namespace IDM.Infrastructure.Services.CatalogService
{
    public class CatalogRootService : ICatalogRootService
    {
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentLoader _contentLoader;
        public CatalogRootService(ReferenceConverter referenceConverter, IContentLoader contentLoader)
        {
            _referenceConverter = referenceConverter;
            _contentLoader = contentLoader;
        }

        public CatalogContentBase GetCatalogRoot()
        {
            var catalogRootReference = _referenceConverter.GetRootLink();

            return _contentLoader.Get<CatalogContentBase>(catalogRootReference);
        }
    }
}
