using EPiServer.ServiceLocation;
using IDM.Shared.Models;

namespace IDM.Application.Services.ContentModel
{
    public class ContentViewModel<TContent> : IContentViewModel<TContent> where TContent : class
    {
        private readonly Injected<MenuService> _menuService;

        public ContentViewModel(TContent currentContent)
        {
            CurrentContent = currentContent;
        }

        public List<WebNavigation> CmsNavigation => _menuService.Service.GetCmsNavigation();
        public List<WebNavigation> CommerceNavigation => _menuService.Service.GetCommerceNavigation();
        public TContent CurrentContent { get; set; }
    }

    public static class ContentViewModel
    {
        public static ContentViewModel<T> Create<T>(T content) where T : class => new ContentViewModel<T>(content);
    }
}
