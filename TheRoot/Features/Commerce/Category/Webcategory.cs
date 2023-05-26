using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.DataAnnotations;

namespace Headless.Features.Commerce.Category
{
    [CatalogContentType(
        DisplayName = "Web Category",
        GUID = "8F54E163-8AE8-4392-A652-5A8EB1D99B27",
        Description = "This is a Web category for default products")]
    public class WebCategory : NodeContent
    {
    }
}
