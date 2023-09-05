using IDM.Application.Features.Commerce.Variants;
using IDM.Application.Features.Commerce.Variants.Models;

namespace IDM.Application.Services.Variant
{
    public interface IVariantService
    {
        VariantModel ToVariantModel(GenericVariant variant);
    }
}
