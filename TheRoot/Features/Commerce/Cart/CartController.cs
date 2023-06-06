using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using IDM.Application.Features.Commerce.Products;
using IDM.Application.Features.Commerce.Variants;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Pricing;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace IDM.Application.Features.Commerce.Cart
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    public class CartController : ApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly CustomerContext _customerContext;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentRepository _contentRepository;
        private readonly IPriceService _priceService;
        private readonly ICurrentMarket _marketService;
        private readonly IContentLoader _contentLoader;
        private readonly IPromotionEngine _promotionEngine;

        public CartController(IOrderRepository orderRepository,
            CustomerContext customerContext,
            IOrderGroupFactory orderGroupFactory,
            ReferenceConverter referenceConverter,
            IContentRepository contentRepository,
            IPriceService priceService,
            ICurrentMarket marketService,
            IContentLoader contentLoader, IPromotionEngine promotionEngine)
        {
            _orderRepository = orderRepository;
            _customerContext = customerContext;
            _orderGroupFactory = orderGroupFactory;
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
            _priceService = priceService;
            _marketService = marketService;
            _contentLoader = contentLoader;
            _promotionEngine = promotionEngine;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(AddToCart))]
        public IActionResult AddToCart(string code, int quantity)
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(_customerContext.CurrentContactId, "Default");
            var lineItem = cart.CreateLineItem(code, _orderGroupFactory);
            SetDefaultLineItemValues(lineItem, quantity);
            cart.AddLineItem(lineItem, _orderGroupFactory);
            cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());
            _orderRepository.Save(cart);
            return new JsonResult(new { cart.Name });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(ConvertToPurchaseOrder))]
        public IActionResult ConvertToPurchaseOrder()
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(_customerContext.CurrentContactId, "Default");
            cart.OrderStatus = OrderStatus.Completed;
            _orderRepository.SaveAsPurchaseOrder(cart);
            return new JsonResult(new { cart.Name });
        }

        private void SetDefaultLineItemValues(ILineItem lineItem, decimal quantity)
        {
            var variation = GetVariation(lineItem.Code);
            if (variation is null)
                return;

            var product = GetProduct(variation);
            lineItem.DisplayName = product?.DisplayName;
            lineItem.Quantity = quantity;
            lineItem.PlacedPrice = Price(variation).FirstOrDefault()!.UnitPrice;
        }

        private GenericVariant? GetVariation(string code)
        {
            var contentLink = _referenceConverter.GetContentLink(code, CatalogContentType.CatalogEntry);
            if (contentLink == ContentReference.EmptyReference)
                return null;

            return _contentRepository.TryGet<GenericVariant>(contentLink, out var variation) ? variation : null;
        }

        public GenericProduct? GetProduct(GenericVariant variationBase)
        {
            var parentProductLink = variationBase.GetParentProducts().FirstOrDefault();
            return _contentLoader.TryGet(parentProductLink, out GenericProduct? product)
                ? product
                : null;
        }

        private List<IPriceValue> Price(GenericVariant variant)
        {
            return _priceService
                .GetCatalogEntryPrices(new CatalogKey(variant.Code))
                .Where(x => x.MarketId == _marketService.GetCurrentMarket().MarketId && x.UnitPrice.Currency == _marketService.GetCurrentMarket().DefaultCurrency)
                .ToList();
        }
    }
}
