using IDM.Application.Features.CMS.Pages.HomePage.Models;

namespace IDM.Application.Features.CMS.Pages.HomePage.Services
{
    public interface IHomePageModelFactory
    {
        HomePageModel Create(HomePage content);
    }
}
