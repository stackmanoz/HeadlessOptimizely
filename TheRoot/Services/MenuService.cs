using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Web.Routing;
using IDM.Application.Features.Commerce.Category;
using IDM.Infrastructure.Services.CatalogService;
using IDM.Shared.Models;

namespace IDM.Application.Services
{
    public class MenuService
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly ICatalogRootService _catalogRootService;
        private readonly IClient _findClient;

        public MenuService(IContentLoader contentRepository,
            UrlResolver urlResolver, ICatalogRootService catalogRootService, IClient findClient)
        {
            _contentLoader = contentRepository;
            _urlResolver = urlResolver;
            _catalogRootService = catalogRootService;
            _findClient = findClient;
        }

        public List<WebNavigation> GetCmsNavigation()
        {
            var navigationItems = new List<WebNavigation>();

            var menuLists =
                _contentLoader
                    .GetChildren<PageData>(ContentReference.RootPage);

            AddMenuRecursive(menuLists, navigationItems);

            return navigationItems;
        }

        public List<WebNavigation> GetCommerceNavigation()
        {
            var navigationItems = new List<WebNavigation>();

            var list = GetWebCategoriesWithSubcategories(_catalogRootService.GetCatalogRoot().CatalogId);

            return list;
        }

        private void AddMenuRecursive(IEnumerable<PageData> menuLists, ICollection<WebNavigation> navigationItems)
        {
            foreach (var page in menuLists)
            {
                if (!page.VisibleInMenu) continue;
                var parentItem = new WebNavigation
                {
                    Name = page.Name,
                    Url = _urlResolver.GetUrl(page.ContentLink),
                };

                AddSubMenuItems(page, parentItem);

                navigationItems.Add(parentItem);
            }
        }

        private void AddSubMenuItems(IContent page, WebNavigation parentItem)
        {
            var menuPages = _contentLoader.GetChildren<IContent>(page.ContentLink);

            foreach (var menuPage in menuPages)
            {
                var navigationItem = new WebNavigation
                {
                    Name = menuPage.Name,
                    Url = _urlResolver.GetUrl(menuPage.ContentLink),
                };

                AddSubMenuItems(menuPage, navigationItem);

                parentItem.Child.Add(navigationItem);
            }
        }

        public IEnumerable<WebNavigation> GetWebCategoriesWithSubcategories(int catalogId)
        {
            var query = _findClient
                .Search<WebCategory>().Filter(x => x.CatalogId.Match(catalogId));
            var results = query.GetResult();

            var webCategories = new List<WebNavigation>();
            foreach (var hit in results)
            {
                if (hit == null) continue;
                webCategories.Add(new WebNavigation()
                {
                    Name = hit.DisplayName,
                    Child = GetSubcategoriesRecursive(results.ToList(), hit),
                    Url = _urlResolver.GetUrl(hit.ContentLink)
                });
            }

            return webCategories;
        }

        private List<WebNavigation> GetSubcategoriesRecursive(List<WebCategory> webCategories, WebCategory parentCategory)
        {
            var query = _findClient
                .Search<WebCategory>().Filter(x => x.ParentLink.ID.Match(parentCategory.ContentLink.ID));
            var results = query.GetResult();

            foreach (var hit in results)
            {
                if (hit == null) continue;
                webCategories.Add(hit);
                GetSubcategoriesRecursive(webCategories, hit);
            }
        }
    }
}
