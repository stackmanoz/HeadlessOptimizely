using System.ComponentModel.DataAnnotations;
using IDM.Application.Features.CMS.Pages.Base;

namespace IDM.Application.Features.CMS.Pages.HomePage
{
    [ContentType(DisplayName = "Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-02.png")]
    public class HomePage : CommonPageData
    {
        [CultureSpecific]
        [Display(Name = "Top content area", GroupName = SystemTabNames.Content, Order = 190)]
        public virtual ContentArea TopContentArea { get; set; }

        [CultureSpecific]
        [Display(Name = "Bottom content area", GroupName = SystemTabNames.Content, Order = 210)]
        public virtual ContentArea BottomContentArea { get; set; }
    }
}
