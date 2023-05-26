using Concrete.Models;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;

namespace Infrastructure.Factories
{
    public class MenuFactory
    {
        private readonly IContentRepository _contentRepository;
        private readonly UrlResolver _urlResolver;

        public MenuFactory(IContentRepository contentRepository,
            UrlResolver urlResolver)
        {
            _contentRepository = contentRepository;
            _urlResolver = urlResolver;
        }

        public List<CustomerNavigationItem> GetNavigationMainMenu()
        {
            var navigationItems = new List<CustomerNavigationItem>();

            var menuLists =
                _contentRepository
                    .GetChildren<PageData>(ContentReference.RootPage);

            AddMenuRecursive(menuLists, navigationItems);

            return navigationItems;

        }

        private void AddMenuRecursive(IEnumerable<PageData> menuLists, ICollection<CustomerNavigationItem> navigationItems)
        {
            foreach (var page in menuLists)
            {
                var parentItem = new CustomerNavigationItem
                {
                    Name = page.Name,
                    Url = _urlResolver.GetUrl(page.ContentLink),
                };

                AddSubMenuItems(page, parentItem);

                navigationItems.Add(parentItem);
            }
        }

        private void AddSubMenuItems(IContent page, CustomerNavigationItem parentItem)
        {
            var menuPages = _contentRepository.GetChildren<PageData>(page.ContentLink);

            foreach (var menuPage in menuPages)
            {
                var navigationItem = new CustomerNavigationItem
                {
                    Name = menuPage.Name,
                    Url = _urlResolver.GetUrl(menuPage.ContentLink),
                };

                AddSubMenuItems(menuPage, navigationItem);

                parentItem.Child.Add(navigationItem);
            }
        }
    }
}
