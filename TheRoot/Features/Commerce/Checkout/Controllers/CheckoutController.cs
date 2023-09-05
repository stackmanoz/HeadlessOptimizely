using EPiServer.Commerce.Order;
using EPiServer.Commerce.UI.Admin.Warehouses.Internal;
using IDM.Application.Features.Commerce.Checkout.Services;
using Klarna.Checkout;
using Klarna.Checkout.Models;
using Klarna.Common.Models;
using Mediachase.Commerce;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.Commerce.Checkout.Controllers
{
    [Route("klarnacheckout")]
    public class CheckoutController : Controller
    {
        private readonly IKlarnaCheckoutService _klarnaCheckoutService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMarketService _marketService;
        private readonly ICurrentMarket _currentMarket;
        private readonly CustomerContext _customerContext;
        private readonly CustomerService _customerService;
        private readonly IOrderGroupFactory _orderGroupFactory;

        public CheckoutController(
            IKlarnaCheckoutService klarnaCheckoutService,
            IOrderRepository orderRepository,
            IMarketService marketService, CustomerContext customerContext, IOrderGroupFactory orderGroupFactory, CustomerService customerService, ICurrentMarket currentMarket)
        {
            _klarnaCheckoutService = klarnaCheckoutService;
            _orderRepository = orderRepository;
            _marketService = marketService;
            _customerContext = customerContext;
            _orderGroupFactory = orderGroupFactory;
            _customerService = customerService;
            _currentMarket = currentMarket;
        }

        [HttpPost]
        [Route(nameof(ProcessOrderPurchaseOrder))]
        public IActionResult ProcessOrderPurchaseOrder()
        {
            try
            {
                var cart = _orderRepository.LoadOrCreateCart<ICart>(_customerService.GetCustomerId(), "Default", _currentMarket);
                cart.OrderStatus = OrderStatus.InProgress;
                SetCartCurrency(cart, cart.Currency);
                _orderRepository.Save(cart);
                var response = _klarnaCheckoutService.CreateOrUpdateOrder(cart);
                var result = response.GetAwaiter().GetResult();
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.StackTrace);
            }
        }

        public void SetCartCurrency(ICart cart, Currency currency)
        {
            if (currency.IsEmpty || currency == cart.Currency)
            {
                return;
            }

            cart.Currency = currency;
            foreach (var lineItem in cart.GetAllLineItems())
            {
                // If there is an item which has no price in the new currency, a NullReference exception will be thrown.
                // Mixing currencies in cart is not allowed.
                // It's up to site's managers to ensure that all items have prices in allowed currency.
                lineItem.PlacedPrice = PriceCalculationService.GetSalePrice(lineItem.Code, cart.MarketId, currency).UnitPrice.Amount;
            }
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


        [Route("cart/{orderGroupId}/shippingoptionupdate")]
        [HttpPost]
        public ActionResult ShippingOptionUpdate(int orderGroupId, [FromBody] ShippingOptionUpdateRequest shippingOptionUpdateRequest)
        {
            var cart = _orderRepository.Load<ICart>(orderGroupId);

            var response = _klarnaCheckoutService.UpdateShippingMethod(cart, shippingOptionUpdateRequest);

            return Ok(response);
        }

        [Route("cart/{orderGroupId}/addressupdate")]
        [AcceptVerbs("POST")]
        [HttpPost]
        public ActionResult AddressUpdate(int orderGroupId, [FromBody] CallbackAddressUpdateRequest addressUpdateRequest)
        {
            var cart = _orderRepository.Load<ICart>(orderGroupId);
            var response = _klarnaCheckoutService.UpdateAddress(cart, addressUpdateRequest);
            return Ok(response);
        }

        [Route("cart/{orderGroupId}/ordervalidation")]
        [AcceptVerbs("POST")]
        [HttpPost]
        public ActionResult OrderValidation(int orderGroupId, [FromBody] CheckoutOrder checkoutData)
        {
            // More information: https://docs.klarna.com/klarna-checkout/popular-use-cases/validate-order/

            //var cart = _orderRepository.Load<ICart>(orderGroupId);

            //// Validate cart lineitems
            //var validationIssues = _cartService.ValidateCart(cart);
            //if (validationIssues.Any())
            //{
            //    // check validation issues and redirect to a page to display the error
            //    return Redirect("/en/error-pages/checkout-something-went-wrong/");
            //}

            // Validate billing address if necessary (this is just an example)
            // To return an error like this you need require_validate_callback_success set to true
            if (checkoutData.BillingCheckoutAddress.PostalCode.Equals("94108-2704"))
            {
                var errorResult = new ErrorResult
                {
                    ErrorType = ErrorType.address_error,
                    ErrorText = "Can't ship to postalcode 94108-2704"
                };
                return BadRequest(errorResult);
            }

            // Validate order amount, shipping address
            //if (!_klarnaCheckoutService.ValidateOrder(cart, checkoutData))
            //{
            //    return Redirect("/en/error-pages/checkout-something-went-wrong/");
            //}

            return Ok();
        }

        [Route("fraud")]
        [AcceptVerbs("POST")]
        [HttpPost]
        public ActionResult FraudNotification(NotificationModel notification)
        {
            _klarnaCheckoutService.FraudUpdate(notification);
            return Ok();
        }

        [Route("cart/{orderGroupId}/push")]
        [AcceptVerbs("POST")]
        [HttpPost]
        public async Task<ActionResult> Push(int orderGroupId, string klarna_order_id)
        {
            if (klarna_order_id == null)
            {
                return BadRequest();
            }

            var purchaseOrder = await GetOrCreatePurchaseOrder(orderGroupId, klarna_order_id).ConfigureAwait(false);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            // Update merchant reference
            await _klarnaCheckoutService.UpdateMerchantReference1(purchaseOrder).ConfigureAwait(false);

            // Acknowledge the order through the order management API
            await _klarnaCheckoutService.AcknowledgeOrder(purchaseOrder);

            return Ok();
        }

        private async Task<IPurchaseOrder> GetOrCreatePurchaseOrder(int orderGroupId, string klarnaOrderId)
        {
            // Check if the order has been created already
            var purchaseOrder = _klarnaCheckoutService.GetPurchaseOrderByKlarnaOrderId(klarnaOrderId);
            if (purchaseOrder != null)
            {
                return purchaseOrder;
            }

            // Check if we still have a cart and can create an order
            var cart = _orderRepository.Load<ICart>(orderGroupId);
            var cartKlarnaOrderId = cart.Properties[Constants.KlarnaCheckoutOrderIdCartField]?.ToString();
            if (cartKlarnaOrderId == null || !cartKlarnaOrderId.Equals(klarnaOrderId))
            {
                return null;
            }

            var market = _marketService.GetMarket(cart.MarketId);
            var order = await _klarnaCheckoutService.GetOrder(klarnaOrderId, market).ConfigureAwait(false);
            if (!order.Status.Equals("checkout_complete"))
            {
                // Won't create order, Klarna checkout not complete
                return null;
            }
            //purchaseOrder = await _checkoutService.CreatePurchaseOrderForKlarna(klarnaOrderId, order, cart).ConfigureAwait(false);
            return purchaseOrder;
        }
    }
}
