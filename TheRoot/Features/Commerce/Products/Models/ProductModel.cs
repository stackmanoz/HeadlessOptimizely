using IDM.Application.Features.Commerce.Variants.Models;
using IDM.Shared.Models;

namespace IDM.Application.Features.Commerce.Products.Models
{
    public class ProductModel : SyncModel
    {
        public ProductModel()
        {
        }

        public ProductModel(string productName, string url, List<ProductImageModel> images, List<VariantModel?> variants)
        {
            ProductName = productName;
            Url = url;
            Images = images;
            Variants = variants;
        }

        public string ProductName { get; set; }
        public string? ProductMainImage => Images.FirstOrDefault()?.ImageName;
        public string Url { get; set; }
        public List<ProductImageModel> Images { get; set; }
        public string? Code => FirstOrDefaultModel?.Code;
        public string? Price => FirstOrDefaultModel?.Price;
        public List<VariantModel?> Variants { get; set; }
        public override string TypeName => "Product";

        private VariantModel? FirstOrDefaultModel => this.Variants.FirstOrDefault();
    }
}
