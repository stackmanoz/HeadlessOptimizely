using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;

namespace Headless.Features.Commerce.Products
{
    [CatalogContentType(
        DisplayName = "Generic Product",
        GUID = "4965d65c-9415-43dd-ad9c-3d4d080fd27d",
        MetaClassName = "Generic Product",
        Description = "")]
    public class GenericProduct : ProductContent
    { }
}
