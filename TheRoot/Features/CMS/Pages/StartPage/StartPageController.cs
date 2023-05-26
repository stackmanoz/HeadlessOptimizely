using EPiServer.Web.Mvc;
using Headless.Features.CMS.Pages.StartPage.Models;
using Infrastructure.Factories;
using Microsoft.AspNetCore.Mvc;

namespace Headless.Features.CMS.Pages.StartPage
{
    public class StartPageController : PageController<StartPage>
    {
        private readonly ViewFactory _viewFactory;
        public StartPageController(ViewFactory viewFactory)
        {
            _viewFactory = viewFactory;
        }

        public IActionResult Index(StartPage currentContent)
        {
            var viewModel = new StartPageModel
            {
                Title = currentContent.Title
            };

            var model = new Func<StartPageModel>(() => viewModel);

            return _viewFactory.RenderView(model);
        }
    }
}
