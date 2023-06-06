using EPiServer.Find;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.CMS.Pages.ProductListingPage
{
    public class ProductListingPageController : PageController<ProductListingPage>
    {
        private readonly IClient _client;
        public ProductListingPageController(IClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Index(ProductListingPage currentPage)
        {
            return View();
        }
    }
}
