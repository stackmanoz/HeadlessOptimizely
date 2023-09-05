using EPiServer.Web.Routing;
using IDM.Application.Features.Commerce.Category;
using IDM.Application.Features.Commerce.Category.Models;
using IDM.Application.Services.Product;

namespace IDM.Application.Services.WebCategoryService;

public class WebCategoryService : IWebCategoryService
{
    private readonly MenuService _menuService;
    private readonly UrlResolver _urlResolver;
    private readonly IProductService _productService;

    public WebCategoryService(MenuService menuService, UrlResolver urlResolver, IProductService productService)
    {
        _menuService = menuService;
        _urlResolver = urlResolver;
        _productService = productService;
    }

    public WebCategoryModel Create(WebCategory category)
    {
        return new WebCategoryModel
        {
            CategoryName = category.DisplayName,
            SubCategories = _menuService.GetSubcategoriesRecursive(category, new()
            {
                Name = category.DisplayName,
                Url = _urlResolver.GetUrl(category.ContentLink)
            }),
            Products = _productService.GetProductsForCategory(category)
        };
    }
}