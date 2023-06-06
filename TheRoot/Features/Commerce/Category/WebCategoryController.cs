using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Category
{
    public class WebCategoryController : CommerceContentController<WebCategory>
    {
        public WebCategoryController()
        {
            
        }
        public async Task<IActionResult> Index(WebCategory category)
        {
            return new JsonResult(new
            {
                Data = category.Name,
            });
        }
    }
}
