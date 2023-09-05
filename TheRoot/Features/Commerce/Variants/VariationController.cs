using IDM.Application.Services.ContentModel;
using IDM.Application.Services.Variant;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Variants
{
    public class VariationController : CommerceContentController<GenericVariant>
    {
        private readonly IVariantService _variantService;

        public VariationController(IVariantService variantService)
        {
            _variantService = variantService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GenericVariant variant)
        {
            return new JsonResult(ContentViewModel.Create(_variantService.ToVariantModel(variant)));
        }
    }
}