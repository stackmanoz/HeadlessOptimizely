using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Products
{
    public class ProductController : CommerceContentController<GenericProduct>
    {
        private readonly UrlResolver _urlResolver;

        public ProductController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }
        public async Task<IActionResult> Index(GenericProduct product)
        {
            return await Task.FromResult(new JsonResult(new
            {
                StatusCode = 200,
                Data = product.Name,
                Url = _urlResolver.GetUrl(product.ContentLink),
                RedirectUrl = $"https://google.com",
            }));
        }
    }
}
