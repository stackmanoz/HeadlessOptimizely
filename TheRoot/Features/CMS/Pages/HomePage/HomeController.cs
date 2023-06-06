using EPiServer.Web.Mvc;
using IDM.Application.Features.CMS.Pages.HomePage.Models;
using IDM.Application.Features.CMS.Pages.HomePage.Services;
using IDM.Application.Services.ContentModel;
using Microsoft.AspNetCore.Mvc;

namespace IDM.Application.Features.CMS.Pages.HomePage;

public class HomeController : PageController<HomePage>
{
    private readonly IHomePageModelFactory _pageModel;

    public HomeController(IHomePageModelFactory pageModel)
    {
        _pageModel = pageModel;
    }

    public IActionResult Index(HomePage currentPage)
    {
        return new JsonResult(ContentViewModel.Create<HomePageModel>(_pageModel.Create(currentPage)));
    }
}