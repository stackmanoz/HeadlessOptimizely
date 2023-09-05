using EPiServer.Commerce.SpecializedProperties;

namespace IDM.Application.Services.CatalogMedia;

public interface ICatalogMediaService
{
    List<ImageData> ToImages(ItemCollection<CommerceMedia>? media);
}