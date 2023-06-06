﻿using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.Security;
using IDM.Application.Features.CMS.Pages.AddressBook;
using IDM.Application.Features.Commerce.Checkout.ViewModel;
using IDM.Shared.Contants;
using IDM.Shared.Models.Address;
using Klarna.Checkout;
using Klarna.Checkout.Models;
using Klarna.Common;
using Klarna.Common.Extensions;
using Klarna.Common.Models;
using MailKit;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Exceptions;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Security;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Constants = Klarna.Checkout.Constants;

namespace IDM.Application.Features.Commerce.Checkout.Services
{
    public class CheckoutService
    {
        private readonly IAddressBookService _addressBookService;
        private readonly IOrderGroupCalculator _orderGroupCalculator;
        private readonly IOrderGroupFactory _orderGroupFactory;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IOrderRepository _orderRepository;
        private readonly IContentRepository _contentRepository;
        private readonly CustomerContext _customerContext;
        private readonly LocalizationService _localizationService;
        private readonly IMailService _mailService;
        private readonly IPromotionEngine _promotionEngine;
        //private readonly ILogger _log = LogManager.GetLogger(typeof(CheckoutService));
        //private readonly ISettingsService _settingsService;
        private readonly ILanguageService _languageService;
        private readonly IKlarnaCheckoutService _klarnaCheckoutService;
        private readonly ICartService _cartService;

        //public AuthenticatedPurchaseValidation AuthenticatedPurchaseValidation { get; private set; }
        //public AnonymousPurchaseValidation AnonymousPurchaseValidation { get; private set; }
        //public CheckoutAddressHandling CheckoutAddressHandling { get; private set; }

        public CheckoutService(
            IAddressBookService addressBookService,
            IOrderGroupFactory orderGroupFactory,
            IOrderGroupCalculator orderGroupCalculator,
            IPaymentProcessor paymentProcessor,
            IOrderRepository orderRepository,
            IContentRepository contentRepository,
            LocalizationService localizationService,
            IMailService mailService,
            IPromotionEngine promotionEngine,
            //ISettingsService settingsService,
            ILanguageService languageService,
            IKlarnaCheckoutService klarnaCheckoutService,
            ICartService cartService)
        {
            _addressBookService = addressBookService;
            _orderGroupFactory = orderGroupFactory;
            _orderGroupCalculator = orderGroupCalculator;
            _paymentProcessor = paymentProcessor;
            _orderRepository = orderRepository;
            _contentRepository = contentRepository;
            _customerContext = CustomerContext.Current;
            _localizationService = localizationService;
            _mailService = mailService;
            _promotionEngine = promotionEngine;

            //AuthenticatedPurchaseValidation = new AuthenticatedPurchaseValidation(_localizationService);
            //AnonymousPurchaseValidation = new AnonymousPurchaseValidation(_localizationService);
            //CheckoutAddressHandling = new CheckoutAddressHandling(_addressBookService);
            //_settingsService = settingsService;
            _languageService = languageService;
            _klarnaCheckoutService = klarnaCheckoutService;
            _cartService = cartService;
        }

        //public virtual void UpdateShippingMethods(ICart cart, IList<ShipmentViewModel> shipmentViewModels)
        //{
        //    var index = 0;
        //    foreach (var shipment in cart.GetFirstForm().Shipments)
        //    {
        //        shipment.ShippingMethodId = shipmentViewModels[index++].ShippingMethodId;
        //    }
        //}

        public virtual void UpdateShippingAddresses(ICart cart, CheckoutViewModel viewModel)
        {
            var shipments = cart.GetFirstForm().Shipments;
            for (var index = 0; index < shipments.Count; index++)
            {
                shipments.ElementAt(index).ShippingAddress = _addressBookService.ConvertToAddress(viewModel.Shipments[index].Address, cart);
            }
        }

        //public virtual void ChangeAddress(ICart cart, CheckoutViewModel viewModel, UpdateAddressViewModel updateAddressViewModel)
        //{
        //    if (updateAddressViewModel.AddressType == AddressType.Billing)
        //    {
        //        foreach (var payment in cart.GetFirstForm().Payments)
        //        {
        //            payment.BillingAddress = _addressBookService.ConvertToAddress(viewModel.BillingAddress, cart);
        //        }
        //    }
        //    else
        //    {
        //        var shipments = cart.GetFirstForm().Shipments;
        //        shipments.ElementAt(updateAddressViewModel.ShippingAddressIndex).ShippingAddress =
        //                _addressBookService.ConvertToAddress(viewModel.Shipments[updateAddressViewModel.ShippingAddressIndex].Address, cart);
        //    }
        //}

        /// <summary>
        /// Update payment plan information
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="viewModel"></param>
        public virtual void UpdatePaymentPlan(ICart cart, CheckoutViewModel viewModel)
        {
            if (viewModel.IsUsePaymentPlan)
            {
                cart.Properties["IsUsePaymentPlan"] = true;
                cart.Properties["PaymentPlanSetting"] = viewModel.PaymentPlanSetting;
            }
            else
            {
                cart.Properties["IsUsePaymentPlan"] = false;
            }
        }

        public virtual void ApplyDiscounts(ICart cart) => cart.ApplyDiscounts(_promotionEngine, new PromotionEngineSettings());

        public virtual void CreateAndAddPaymentToCart(ICart cart, CheckoutViewModel viewModel)
        {
            var total = viewModel.OrderSummary.PaymentTotal;
            var paymentMethod = viewModel.Payment;
            if (paymentMethod == null)
            {
                return;
            }

            var payment = cart.GetFirstForm().Payments.FirstOrDefault(x => x.PaymentMethodId == paymentMethod.PaymentMethodId);

            if (payment == null)
            {
                payment = paymentMethod.CreatePayment(total, cart);

                cart.AddPayment(payment, _orderGroupFactory);
            }
            else
            {
                payment.Amount = viewModel.OrderSummary.PaymentTotal;
            }

            //if (payment.PaymentMethodName.Equals(Klarna.Payments.Constants.KlarnaPaymentSystemKeyword))
            //{
            //    payment.Properties[Klarna.Payments.Constants.AuthorizationTokenPaymentField] = viewModel.AuthorizationToken;
            //}
        }

        public virtual void RemovePaymentFromCart(ICart cart, CheckoutViewModel viewModel)
        {
            var paymentMethod = viewModel.Payment;
            if (paymentMethod == null)
            {
                return;
            }

            var payment = cart.GetFirstForm().Payments.FirstOrDefault(x => x.PaymentMethodId == paymentMethod.PaymentMethodId);
            cart.GetFirstForm().Payments.Remove(payment);
        }

        public virtual IPurchaseOrder PlaceOrder(ICart cart, ModelStateDictionary modelState, CheckoutViewModel checkoutViewModel)
        {
            try
            {
                if (cart.Properties[IDMConstants.Quote.ParentOrderGroupId] != null)
                {
                    var orderLink = int.Parse(cart.Properties[IDMConstants.Quote.ParentOrderGroupId].ToString());
                    if (orderLink != 0)
                    {
                        var quoteOrder = _orderRepository.Load<IPurchaseOrder>(orderLink);
                        if (quoteOrder.Properties[IDMConstants.Quote.QuoteStatus] != null)
                        {
                            checkoutViewModel.QuoteStatus = quoteOrder.Properties[IDMConstants.Quote.QuoteStatus].ToString();
                            if (quoteOrder.Properties[IDMConstants.Quote.QuoteStatus].ToString().Equals(IDMConstants.Quote.RequestQuotationFinished))
                            {
                                _ = DateTime.TryParse(quoteOrder.Properties[IDMConstants.Quote.QuoteExpireDate].ToString(),
                                    out var quoteExpireDate);
                                if (DateTime.Compare(DateTime.Now, quoteExpireDate) > 0)
                                {
                                    _orderRepository.Delete(cart.OrderLink);
                                    _orderRepository.Delete(quoteOrder.OrderLink);
                                    throw new InvalidOperationException("Quote Expired");
                                }
                            }
                        }
                    }
                }

                var processPayments = cart.ProcessPayments(_paymentProcessor, _orderGroupCalculator);
                var unsuccessPayments = processPayments.Where(x => !x.IsSuccessful);
                if (unsuccessPayments != null && unsuccessPayments.Any())
                {
                    throw new InvalidOperationException(string.Join("\n", unsuccessPayments.Select(x => x.Message)));
                }

                var processedPayments = cart.GetFirstForm().Payments.Where(x => x.Status.Equals(PaymentStatus.Processed.ToString()));

                if (!processedPayments.Any())
                {
                    // Return null in case there is no payment was processed.
                    return null;
                }

                var totalProcessedAmount = processedPayments.Sum(x => x.Amount);
                if (totalProcessedAmount != cart.GetTotal(_orderGroupCalculator).Amount)
                {
                    throw new InvalidOperationException("Wrong amount");
                }

                var orderReference = cart.Properties["IsUsePaymentPlan"] != null && cart.Properties["IsUsePaymentPlan"].Equals(true) ? SaveAsPaymentPlan(cart) : _orderRepository.SaveAsPurchaseOrder(cart);
                var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId);
                _orderRepository.Delete(cart.OrderLink);

                cart.AdjustInventoryOrRemoveLineItems((item, validationIssue) => { });

                //Loyalty Program: Add Points and Number of orders
                //_loyaltyService.AddNumberOfOrders();

                return purchaseOrder;
            }
            catch (PaymentException ex)
            {
                modelState.AddModelError("", _localizationService.GetString("/Checkout/Payment/Errors/ProcessingPaymentFailure") + ex.Message);
            }
            catch (Exception ex)
            {
                modelState.AddModelError("", ex.Message);
            }

            return null;
        }

        public async Task<IPurchaseOrder> CreatePurchaseOrderForKlarna(string klarnaOrderId, CheckoutOrder order, ICart cart)
        {
            var paymentRow = PaymentManager.GetPaymentMethodBySystemName(Constants.KlarnaCheckoutSystemKeyword, _languageService.GetPreferredCulture().Name).PaymentMethod.FirstOrDefault();

            var payment = cart.CreatePayment(_orderGroupFactory);
            payment.PaymentType = PaymentType.Other;
            payment.PaymentMethodId = paymentRow.PaymentMethodId;
            payment.PaymentMethodName = Constants.KlarnaCheckoutSystemKeyword;
            payment.Amount = cart.GetTotal(_orderGroupCalculator).Amount;
            payment.Status = PaymentStatus.Pending.ToString();
            payment.TransactionType = TransactionType.Authorization.ToString();
            payment.ProviderTransactionID = klarnaOrderId;

            cart.AddPayment(payment, _orderGroupFactory);

            var billingAddress = new AddressModel
            {
                Name = $"{order.BillingCheckoutAddress.StreetAddress}{order.BillingCheckoutAddress.StreetAddress2}{order.BillingCheckoutAddress.City}",
                FirstName = order.BillingCheckoutAddress.GivenName,
                LastName = order.BillingCheckoutAddress.FamilyName,
                Email = order.BillingCheckoutAddress.Email,
                DaytimePhoneNumber = order.BillingCheckoutAddress.Phone,
                Line1 = order.BillingCheckoutAddress.StreetAddress,
                Line2 = order.BillingCheckoutAddress.StreetAddress2,
                PostalCode = order.BillingCheckoutAddress.PostalCode,
                City = order.BillingCheckoutAddress.City,
                CountryName = order.BillingCheckoutAddress.Country
            };

            payment.BillingAddress = _addressBookService.ConvertToAddress(billingAddress, cart);

            cart.ProcessPayments(_paymentProcessor, _orderGroupCalculator);

            var totalProcessedAmount = cart.GetFirstForm().Payments.Where(x => x.Status.Equals(PaymentStatus.Processed.ToString())).Sum(x => x.Amount);
            if (totalProcessedAmount != cart.GetTotal(_orderGroupCalculator).Amount)
            {
                throw new InvalidOperationException("Wrong amount");
            }

            if (payment.HasFraudStatus(FraudStatus.PENDING))
            {
                payment.Status = PaymentStatus.Pending.ToString();
            }

            _cartService.RequestInventory(cart);

            var orderReference = _orderRepository.SaveAsPurchaseOrder(cart);
            var purchaseOrder = _orderRepository.Load<IPurchaseOrder>(orderReference.OrderGroupId);
            _orderRepository.Delete(cart.OrderLink);

            if (purchaseOrder == null)
            {
                await _klarnaCheckoutService.CancelOrder(cart).ConfigureAwait(false);

                return null;
            }
            else
            {
                _klarnaCheckoutService.Complete(purchaseOrder);
                purchaseOrder.Properties[Klarna.Common.Constants.KlarnaOrderIdField] = klarnaOrderId;

                _orderRepository.Save(purchaseOrder);
                return purchaseOrder;
            }
        }

        //public virtual string BuildRedirectionUrl(CheckoutViewModel checkoutViewModel, IPurchaseOrder purchaseOrder, bool confirmationSentSuccessfully)
        //{
        //    var queryCollection = new NameValueCollection
        //    {
        //        {"contactId", _customerContext.CurrentContactId.ToString()},
        //        {"orderNumber", purchaseOrder.OrderLink.OrderGroupId.ToString(CultureInfo.CurrentCulture)}
        //    };

        //    if (!confirmationSentSuccessfully)
        //    {
        //        queryCollection.Add("notificationMessage", string.Format(_localizationService.GetString("/OrderConfirmationMail/ErrorMessages/SmtpFailure"), checkoutViewModel.BillingAddress?.Email));
        //    }

        //    var referenceSettings = _settingsService.GetSiteSettings<ReferencePageSettings>();
        //    var confirmationPage = referenceSettings?.OrderConfirmationPage ?? ContentReference.EmptyReference;
        //    if (ContentReference.IsNullOrEmpty(confirmationPage))
        //    {
        //        return null;
        //    }

        //    return new UrlBuilder(UrlResolver.Current.GetUrl(confirmationPage)) { QueryCollection = queryCollection }.ToString();
        //}

        #region Payment Plan

        /// <summary>
        /// Save cart as payment plan
        /// </summary>
        /// <param name="cart"></param>
        private OrderReference SaveAsPaymentPlan(ICart cart)
        {
            var orderReference = _orderRepository.SaveAsPaymentPlan(cart);
            var paymentPlanSetting = cart.Properties["PaymentPlanSetting"] as PaymentPlanSetting;

            IPaymentPlan paymentPlan;
            paymentPlan = _orderRepository.Load<IPaymentPlan>(orderReference.OrderGroupId);
            paymentPlan.CycleMode = PaymentPlanCycle.Days;
            paymentPlan.CycleLength = paymentPlanSetting.CycleLength;
            paymentPlan.StartDate = DateTime.Now.AddDays(paymentPlanSetting.CycleLength);
            paymentPlan.EndDate = paymentPlanSetting.EndDate;
            paymentPlan.IsActive = paymentPlanSetting.IsActive;

            var principal = PrincipalInfo.CurrentPrincipal;
            AddNoteToCart(paymentPlan, $"Note: New payment plan placed by {principal.Identity.Name} in 'vnext site'.", OrderNoteTypes.System.ToString(), principal.GetContactId());

            _orderRepository.Save(paymentPlan);

            paymentPlan.AdjustInventoryOrRemoveLineItems((item, validationIssue) => { });
            _orderRepository.Save(paymentPlan);

            //create first order
            orderReference = _orderRepository.SaveAsPurchaseOrder(paymentPlan);
            var purchaseOrder = _orderRepository.Load(orderReference);
            OrderGroupWorkflowManager.RunWorkflow((OrderGroup)purchaseOrder, OrderGroupWorkflowManager.CartCheckOutWorkflowName);
            var noteDetailPattern = "New purchase order placed by {0} in {1} from payment plan {2}";
            var noteDetail = string.Format(noteDetailPattern, PrincipalInfo.CurrentPrincipal.Identity.Name, "VNext site", (paymentPlan as PaymentPlan).Id);
            AddNoteToPurchaseOrder(purchaseOrder as IPurchaseOrder, noteDetail, OrderNoteTypes.System, PrincipalInfo.CurrentPrincipal.GetContactId());
            _orderRepository.Save(purchaseOrder);

            paymentPlan.LastTransactionDate = DateTime.UtcNow;
            paymentPlan.CompletedCyclesCount++;
            _orderRepository.Save(paymentPlan);

            return orderReference;
        }

        /// <summary>
        /// Add note to purchase order
        /// </summary>
        /// <param name="purchaseOrder"></param>
        /// <param name="noteDetails"></param>
        /// <param name="type"></param>
        /// <param name="customerId"></param>
        private void AddNoteToPurchaseOrder(IPurchaseOrder purchaseOrder, string noteDetails, OrderNoteTypes type, Guid customerId)
        {
            if (purchaseOrder == null)
            {
                throw new ArgumentNullException(nameof(purchaseOrder));
            }

            var orderNote = purchaseOrder.CreateOrderNote();

            if (!orderNote.OrderNoteId.HasValue)
            {
                var newOrderNoteId = -1;

                if (purchaseOrder.Notes.Count != 0)
                {
                    newOrderNoteId = Math.Min(purchaseOrder.Notes.ToList().Min(n => n.OrderNoteId.Value), 0) - 1;
                }

                orderNote.OrderNoteId = newOrderNoteId;
            }

            orderNote.CustomerId = customerId;
            orderNote.Type = type.ToString();
            orderNote.Title = noteDetails.Substring(0, Math.Min(noteDetails.Length, 24)) + "...";
            orderNote.Detail = noteDetails;
            orderNote.Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Add note to cart
        /// </summary>
        /// <param name="cart"></param>
        /// <param name="noteDetails"></param>
        /// <param name="type"></param>
        /// <param name="originator"></param>
        private void AddNoteToCart(IOrderGroup cart, string noteDetails, string type, Guid originator)
        {
            var note = new OrderNote
            {
                CustomerId = originator,
                Type = type,
                Title = noteDetails.Substring(0, Math.Min(noteDetails.Length, 24)) + "...",
                Detail = noteDetails,
                Created = DateTime.UtcNow
            };
            cart.Notes.Add(note);
        }
        #endregion

    }
}
