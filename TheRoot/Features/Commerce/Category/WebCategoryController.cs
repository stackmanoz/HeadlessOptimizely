using IDM.Application.Services.ContentModel;
using IDM.Application.Services.WebCategoryService;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Category;

public class WebCategoryController : CommerceContentController<WebCategory>
{
    private readonly IWebCategoryService _factory;
    public WebCategoryController(IWebCategoryService factory)
    {
        _factory = factory;
    }
    public async Task<IActionResult> Index(WebCategory category)
    {
        return new JsonResult(ContentViewModel.Create(_factory.Create(category)));
    }
}