using IDM.Application.Services.ContentModel;
using IDM.Application.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Products
{
    public class ProductController : CommerceContentController<GenericProduct>
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index(GenericProduct product)
        {
            return new JsonResult(ContentViewModel.Create(_productService.ToProductModel(product)));
        }
    }
}
