using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.OpenIDConnect;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Catalog;
using Microsoft.AspNetCore.Authorization;

namespace Headless.Features.Commerce
{
    [Authorize(AuthenticationSchemes = OpenIDConnectOptionsDefaults.AuthenticationScheme)]
    public abstract class CommerceContentController<T> : ContentController<T> where T : CatalogContentBase
    {
        private Injected<IContentLoader> _contentLoader;
        private Injected<ReferenceConverter> _referenceConverter;

        public IContentLoader ContentLoader { get { return _contentLoader.Service; } }

        public ReferenceConverter ReferenceConverter { get { return _referenceConverter.Service; } }
    }
}
