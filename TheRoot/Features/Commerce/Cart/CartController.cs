using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.UI.Admin.Shipping.Internal;
using EPiServer.Commerce.UI.Admin.Warehouses.Internal;
using IDM.Application.Features.Commerce.Checkout.Helpers;
using IDM.Application.Features.Commerce.Products;
using IDM.Application.Features.Commerce.Variants;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Pricing;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using IDM.Application.Features.Commerce.Checkout.Services;

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
        private readonly IShippingService _shippingService;
        private readonly ILineItemCalculator _lineItemCalculator;
        private readonly CustomerService _customerService;

        public CartController(IOrderRepository orderRepository,
            CustomerContext customerContext,
            IOrderGroupFactory orderGroupFactory,
            ReferenceConverter referenceConverter,
            IContentRepository contentRepository,
            IPriceService priceService,
            ICurrentMarket marketService,
            IContentLoader contentLoader, IPromotionEngine promotionEngine, IShippingService shippingService, ILineItemCalculator lineItemCalculator, CustomerService customerService)
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
            _shippingService = shippingService;
            _lineItemCalculator = lineItemCalculator;
            _customerService = customerService;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(AddToCart))]
        public IActionResult AddToCart(string code, int quantity)
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(_customerService.GetCustomerId(), "Default");
            var variation = GetVariation(code);

            if (variation != null) cart = AddToCart(cart, variation, 1);

            cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());
            _orderRepository.Save(cart);
            return new JsonResult(new { cart.Name });
        }

        public ICart AddToCart(ICart cart, EntryContentBase entryContent, decimal quantity)
        {
            var result = new AddToCartResult();
            var shippingMethod = _shippingService.ListShippingMethods("sv").FirstOrDefault(x => x.IsDefault && x.IsActive);

            var form = cart.GetFirstForm();
            if (form == null)
            {
                form = _orderGroupFactory.CreateOrderForm(cart);
                form.Name = cart.Name;
                cart.Forms.Add(form);
            }

            var shipment = cart.GetFirstShipment();

            if (shipment == null || shipment.ShippingMethodId == Guid.Empty)
            {
                shipment.ShippingMethodId = shippingMethod.ShippingMethodId.GetValueOrDefault(Guid.Empty);
            }

            var lineItem = cart.GetAllLineItems().FirstOrDefault(x => x.Code == entryContent.Code);

            if (lineItem == null)
            {
                lineItem = cart.CreateLineItem(entryContent.Code, _orderGroupFactory);
                lineItem.DisplayName = entryContent.DisplayName;
                lineItem.Quantity = quantity;
                cart.AddLineItem(shipment, lineItem);
            }
            else
            {
                cart.UpdateLineItemQuantity(shipment, lineItem, lineItem.Quantity + quantity);
            }

            return cart;
        }

        private IOrderAddress GetOrderAddressFromWarehosue(ICart cart, WarehouseModel warehouse)
        {
            var address = _orderGroupFactory.CreateOrderAddress(cart);
            address.Id = warehouse.Code;
            address.City = warehouse.ContactInformation.City;
            address.CountryCode = warehouse.ContactInformation.CountryCode;
            address.CountryName = warehouse.ContactInformation.CountryName;
            address.DaytimePhoneNumber = warehouse.ContactInformation.DaytimePhoneNumber;
            address.Email = warehouse.ContactInformation.Email;
            address.EveningPhoneNumber = warehouse.ContactInformation.EveningPhoneNumber;
            address.FaxNumber = warehouse.ContactInformation.FaxNumber;
            address.FirstName = warehouse.ContactInformation.FirstName;
            address.LastName = warehouse.ContactInformation.LastName;
            address.Line1 = warehouse.ContactInformation.Line1;
            address.Line2 = warehouse.ContactInformation.Line2;
            address.Organization = warehouse.ContactInformation.Organization;
            address.PostalCode = warehouse.ContactInformation.PostalCode;
            address.RegionName = warehouse.ContactInformation.RegionName;
            address.RegionCode = warehouse.ContactInformation.RegionCode;
            return address;
        }


        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(ConvertToPurchaseOrder))]
        public IActionResult ConvertToPurchaseOrder()
        {
            var cart = _orderRepository.LoadOrCreateCart<ICart>(_customerService.GetCustomerId(), "Default");
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
