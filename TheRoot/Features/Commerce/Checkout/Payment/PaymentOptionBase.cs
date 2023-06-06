using EPiServer.Commerce.Order;
using EPiServer.Commerce.UI.Admin.Payments.Internal;
using EPiServer.Framework.Localization;
using IDM.Application.Features.Commerce.Checkout.Services;
using Mediachase.Commerce;

namespace IDM.Application.Features.Commerce.Checkout.Payment
{
    public abstract class PaymentOptionBase : IPaymentMethod
    {
        protected readonly LocalizationService LocalizationService;
        protected readonly IOrderGroupFactory OrderGroupFactory;

        public Guid PaymentMethodId { get; }
        public abstract string SystemKeyword { get; }
        public string Name { get; }
        public string Description { get; }
        public Money Amount { get; set; }

        protected PaymentOptionBase(LocalizationService localizationService,
            IOrderGroupFactory orderGroupFactory,
            ICurrentMarket currentMarket,
            ICustomPaymentService paymentService)
        {
            LocalizationService = localizationService;
            OrderGroupFactory = orderGroupFactory;

            if (!string.IsNullOrEmpty(SystemKeyword))
            {
                var currentMarketId = currentMarket.GetCurrentMarket().MarketId.Value;
                var currentLanguage = currentMarket.GetCurrentMarket().DefaultLanguage.TwoLetterISOLanguageName;
                var availablePaymentMethods = paymentService.GetPaymentMethodsByMarketIdAndLanguageCode(currentMarketId, currentLanguage);
                var paymentMethod = availablePaymentMethods.FirstOrDefault(m => m.SystemKeyword.Equals(SystemKeyword));

                if (paymentMethod != null)
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId.GetValueOrDefault();
                    Name = paymentMethod.Name;
                    Description = paymentMethod.Description;
                }
            }
        }

        public abstract IPayment CreatePayment(decimal amount, IOrderGroup orderGroup);

        public abstract bool ValidateData();
    }
}