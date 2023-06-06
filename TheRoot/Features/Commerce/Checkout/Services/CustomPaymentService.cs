using EPiServer.Commerce.UI.Admin.Payments.Internal;
using IDM.Shared.Contants;
using IDM.Shared.Enums;
using IDM.Shared.Extensions;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Orders.Managers;

namespace IDM.Application.Features.Commerce.Checkout.Services
{
    public class CustomPaymentService : ICustomPaymentService
    {
        private readonly CustomerContext _customerContext;

        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomPaymentService(ICartService cartService,
            IHttpContextAccessor httpContextAccessor)
        {
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
            _customerContext = CustomerContext.Current;
        }

        public IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode)
        {
            var methods = PaymentManager.GetPaymentMethodsByMarket(marketId)
                .PaymentMethod
                .Where(x => x.IsActive && languageCode.Equals(x.LanguageId, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Ordering)
                .Select(x => new PaymentMethodViewModel
                {
                    PaymentMethodId = x.PaymentMethodId,
                    SystemKeyword = x.SystemKeyword,
                    Name = x.Name,
                    Description = x.Description,
                    IsDefault = x.IsDefault
                });

            if (_httpContextAccessor.HttpContext == null || !EPiServer.Security.PrincipalInfo.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return methods.Where(payment => !payment.SystemKeyword.Equals(IDMConstants.Order.BudgetPayment));
            }

            var currentContact = new CommerceContact(_customerContext.CurrentContact);
            if (string.IsNullOrEmpty(currentContact.UserRole))
            {
                return methods.Where(payment => !payment.SystemKeyword.Equals(IDMConstants.Order.BudgetPayment));
            }

            var cart = _cartService.LoadCart(_cartService.DefaultCartName, true)?.Cart;
            if (cart != null && cart.IsQuoteCart() && currentContact.B2BUserRole == B2BUserRoles.Approver)
            {
                return methods.Where(payment => payment.SystemKeyword.Equals(IDMConstants.Order.BudgetPayment));
            }

            return currentContact.B2BUserRole == B2BUserRoles.Purchaser ? methods : methods.Where(payment => !payment.SystemKeyword.Equals(IDMConstants.Order.BudgetPayment));
        }
    }
}
