using IDM.Application.Features.Commerce.Category;
using IDM.Application.Features.Commerce.Category.Models;

namespace IDM.Application.Services.WebCategoryService;

public interface IWebCategoryService
{
    WebCategoryModel Create(WebCategory category);
}