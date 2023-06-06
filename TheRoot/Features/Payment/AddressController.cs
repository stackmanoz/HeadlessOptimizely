using IDM.Payment.Address;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;

namespace Headless.Features.Payment
{
    [Microsoft.AspNetCore.Mvc.Route("api/[Controller]")]
    public class AddressController : ApiController
    {
        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route(nameof(AddressUpdate))]
        public IActionResult AddressUpdate(CheckoutAddressResponseModel.Root request)
        {
            return new JsonResult(new CheckoutAddressResponseModel.Root());
        }
    }
}
