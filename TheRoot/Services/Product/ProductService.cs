using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Helpers;
using EPiServer.Web.Routing;
using IDM.Application.Features.Commerce.Category;
using IDM.Application.Features.Commerce.Products;
using IDM.Application.Features.Commerce.Products.Extensions;
using IDM.Application.Features.Commerce.Products.Models;
using IDM.Application.Features.Commerce.Variants;
using IDM.Application.Features.Commerce.Variants.Models;
using IDM.Application.Services.CatalogMedia;
using IDM.Application.Services.Variant;
using Mediachase.Commerce;
using Mediachase.Commerce.Pricing;

namespace IDM.Application.Services.Product;

public class ProductService : IProductService
{
    private readonly IClient _findClient;
    private readonly ICatalogMediaService _catalogMediaService;
    private readonly IContentLoader _contentLoader;
    private readonly ICurrentMarket _currentMarket;
    private readonly IVariantService _variantService;
    private readonly UrlResolver _urlResolver;

    public ProductService(IClient findClient, UrlResolver urlResolver, ICatalogMediaService catalogMediaService, IContentLoader contentLoader, IVariantService variantService, IPriceService priceService, ICurrentMarket marketService, ICurrentMarket currentMarket)
    {
        _findClient = findClient;
        _urlResolver = urlResolver;
        _catalogMediaService = catalogMediaService;
        _contentLoader = contentLoader;
        _variantService = variantService;
        _currentMarket = currentMarket;
    }

    public virtual List<ProductModel> GetProductsForCategory(WebCategory category)
    {
        var contentLink = category.ContentLink.ToReferenceWithoutVersion().ToString();

        var query = _findClient.Search<GenericProduct>()
            .Filter(x => x.AllParentCategoryIds().Match(contentLink))
            .Filter(x=>x.MatchMarketId(_currentMarket.GetCurrentMarket().MarketId))
            .ExcludeDeleted()
            .PublishedInLanguage(category.Language.Name)
            .StaticallyCacheFor(TimeSpan.FromSeconds(15));

        return query
            .GetContentResult()
            .Select(ToProductModel).ToList();
    }

    public virtual ProductModel ToProductModel(GenericProduct product)
    {
        return new ProductModel()
        {
            Url = _urlResolver.GetUrl(product.ContentLink, product.Language.Name),
            ProductName = product.DisplayName,
            Images = _catalogMediaService.ToImages(product.CommerceMediaCollection)
                .Select(x => new ProductImageModel()
                {
                    ImageName = x.Name,
                    ImageUrl = _urlResolver.GetUrl(x.ContentLink)
                }).ToList(),
            Variants = ProductVariants(product),
        };
    }

    public virtual List<VariantModel> ProductVariants(GenericProduct product)
    {
        var results = new List<VariantModel>();
        product.Variations().ForEach(v =>
        {
            var variant = _contentLoader.Get<GenericVariant>(v);
            results.Add(_variantService.ToVariantModel(variant));
        });

        return results;
    }
}