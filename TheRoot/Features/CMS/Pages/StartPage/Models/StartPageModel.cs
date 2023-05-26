using Concrete.Interfaces;

namespace Headless.Features.CMS.Pages.StartPage.Models
{
    public class StartPageModel : IModel
    {
        public string PageType => "StartPage";
        public string Title { get; set; }
    }
}
