using EPiServer.Commerce.UI.Admin.Payments.Internal;

namespace IDM.Application.Features.Commerce.Checkout.Services
{
    public interface ICustomPaymentService
    {
        IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode);
    }
}
