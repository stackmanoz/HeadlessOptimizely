using IDM.Application.Features.CMS.Pages.HomePage.Models;

namespace IDM.Application.Features.CMS.Pages.HomePage.Services
{
    public class HomePageFactory : IHomePageModelFactory
    {
        public HomePageModel Create(HomePage content)
        {
            //todo:use page to create model
            return new HomePageModel();
        }
    }
}
