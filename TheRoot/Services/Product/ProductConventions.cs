using EPiServer.Find.ClientConventions;
using IDM.Application.Features.Commerce.Products;
using IDM.Application.Features.Commerce.Products.Extensions;

namespace IDM.Application.Services.Product
{
    public static class ProductConventions
    {
        public static TypeConventionBuilder<GenericProduct> ApplyProductConventions(
            this TypeConventionBuilder<GenericProduct> builder)
        {
            builder.IncludeField(x => x.AllCategoryIds());
            builder.IncludeField(x => x.AllParentCategoryIds());
            return builder;
        }
    }
}
