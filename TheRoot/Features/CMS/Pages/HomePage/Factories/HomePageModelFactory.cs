using IDM.Application.Features.CMS.Pages.HomePage.Models;
using IDM.Application.Features.CMS.Pages.HomePage.Services;

namespace IDM.Application.Features.CMS.Pages.HomePage.Factories
{
    public class HomePageModelFactory : IHomePageModelFactory
    {
        public HomePageModel Create(HomePage content)
        {
            return new HomePageModel();
        }
    }
}
