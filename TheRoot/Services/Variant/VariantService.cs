using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Web.Routing;
using IDM.Application.Features.Commerce.Variants;
using IDM.Application.Features.Commerce.Variants.Models;
using IDM.Application.Services.CatalogMedia;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;

namespace IDM.Application.Services.Variant;

public class VariantService : IVariantService
{
    private readonly ICatalogMediaService _catalogMediaService;
    private readonly IPriceService _priceService;
    private readonly ICurrentMarket _marketService;
    private readonly UrlResolver _urlResolver;

    public VariantService(ICatalogMediaService catalogMediaService,
        UrlResolver urlResolver,
        IPriceService priceService,
        ICurrentMarket marketService)
    {
        _catalogMediaService = catalogMediaService;
        _urlResolver = urlResolver;
        _priceService = priceService;
        _marketService = marketService;
    }

    public VariantModel ToVariantModel(GenericVariant variant)
    {
        return new VariantModel
        {
            Url = _urlResolver.GetUrl(variant.ContentLink, variant.Language.Name),
            VariantName = variant.DisplayName,
            Images = _catalogMediaService.ToImages(variant.CommerceMediaCollection)
                .Select(x => new VariantImageModel()
                {
                    ImageName = x.Name,
                    ImageUrl = _urlResolver.GetUrl(x.ContentLink)
                }).ToList(),
            Price = Price(variant).FirstOrDefault()?.UnitPrice.Amount.ToString("0.00"),
            Code = variant.Code,
            Color = variant.Color,
            Size = variant.Size,
        };
    }

    private IEnumerable<IPriceValue> Price(EntryContentBase product)
    {
        return _priceService
            .GetCatalogEntryPrices(new CatalogKey(product.Code))
            .Where(x => x.MarketId == _marketService.GetCurrentMarket().MarketId && x.UnitPrice.Currency == _marketService.GetCurrentMarket().DefaultCurrency)
            .ToList();
    }
}