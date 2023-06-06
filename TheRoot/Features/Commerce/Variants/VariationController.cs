using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Variants
{
    public class VariationController : CommerceContentController<GenericVariant>
    {
        private readonly UrlResolver _urlResolver;

        public VariationController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GenericVariant product)
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