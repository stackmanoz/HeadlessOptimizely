using IDM.Shared.Models;

namespace IDM.Application.Features.Commerce.Variants.Models;

public class VariantModel : SyncModel
{
    public string VariantName { get; set; }
    public string Url { get; set; }
    public List<VariantImageModel> Images { get; set; }
    public override string TypeName => "Variant";
    public string? Code { get; set; }
    public string? Price { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
}