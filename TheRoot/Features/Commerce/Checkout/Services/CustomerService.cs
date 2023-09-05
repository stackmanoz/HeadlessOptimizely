using EPiServer.Security;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Security;

namespace IDM.Application.Features.Commerce.Checkout.Services
{
    public class CustomerService : CustomerContext
    {
        private const string CookieKey = "AnonymousId";

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCustomerId()
        {
            try
            {
                if (PrincipalInfo.CurrentPrincipal.Identity is { IsAuthenticated: true })
                {
                    var primaryKeyId = PrincipalInfo.CurrentPrincipal.GetCustomerContact().PrimaryKeyId;
                    if (primaryKeyId != null)
                        return primaryKeyId.Value;
                }

                var cookie = _httpContextAccessor.HttpContext?.Request.Headers[CookieKey].ToString();
                if (!string.IsNullOrEmpty(cookie)) return new Guid(cookie);
                var newId = _httpContextAccessor.HttpContext == null
                    ? new Guid()
                    : new Guid(_httpContextAccessor.HttpContext.Session.Id);

                return newId;
            }
            catch
            {
                return new Guid();
            }
        }
    }
}
