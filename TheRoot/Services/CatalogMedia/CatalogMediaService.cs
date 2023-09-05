using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Find.Helpers;

namespace IDM.Application.Services.CatalogMedia;

public class CatalogMediaService : ICatalogMediaService
{
    private readonly IContentLoader _contentLoader;
    public CatalogMediaService(IContentLoader contentLoader)
    {
        _contentLoader = contentLoader;
    }
    public List<ImageData> ToImages(ItemCollection<CommerceMedia>? media)
    {
        var result = new List<ImageData>();
        if (media == null) return result;
        media.ForEach(c =>
        {
            var item = _contentLoader.Get<ImageData>(c.AssetLink);
            result.Add(item);
        });
        return result;
    }
}