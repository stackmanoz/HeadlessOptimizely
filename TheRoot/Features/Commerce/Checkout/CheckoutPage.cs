using IDM.Application.Features.CMS.Pages.Base;
using static IDM.Shared.Contants.IDMConstants;

namespace IDM.Application.Features.Commerce.Checkout
{
    [ContentType(DisplayName = "Checkout Page",
        GUID = "6709cd32-7bb6-4d29-9b0b-207369799f4f",
        Description = "Checkout page",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("/icons/cms/pages/cms-icon-page-08.png")]
    public class CheckoutPage : CommonPageData
    {
    }
}
