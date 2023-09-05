using IDM.Application.Features.Commerce.Products;
using IDM.Application.Features.Commerce.Products.Models;

namespace IDM.Application.Services.Product;

public interface IProductService
{
    List<ProductModel> GetProductsForCategory(Features.Commerce.Category.WebCategory category);
    ProductModel ToProductModel(GenericProduct product);
}