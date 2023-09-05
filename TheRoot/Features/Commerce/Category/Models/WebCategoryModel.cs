using IDM.Application.Features.Commerce.Products.Models;
using IDM.Shared.Models;

namespace IDM.Application.Features.Commerce.Category.Models;

public class WebCategoryModel : SyncModel
{
    public string CategoryName { get; set; }
    public virtual WebNavigation SubCategories { get; set; }
    public virtual List<ProductModel> Products { get; set; }
    public override string TypeName => "Category";
}