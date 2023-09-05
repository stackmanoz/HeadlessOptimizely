using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Web.Routing;
using IDM.Application.Features.Commerce.Category;
using IDM.Shared.Models;
using Mediachase.Commerce.Catalog;

namespace IDM.Application.Services
{
    public class MenuService
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IClient _findClient;

        public MenuService(IContentLoader contentRepository,
            UrlResolver urlResolver, IClient findClient, ReferenceConverter referenceConverter)
        {
            _contentLoader = contentRepository;
            _urlResolver = urlResolver;
            _findClient = findClient;
            _referenceConverter = referenceConverter;
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
            var rootLink = _referenceConverter.GetRootLink();
            var catalogRef = _contentLoader.GetChildren<CatalogContent>(rootLink)
                .FirstOrDefault(x => x.Name.ToLower() == "bharat");

            var list = GetWebCategoriesWithSubcategories(catalogRef.ContentLink.ID);

            return list.ToList();
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
                .Search<WebCategory>().Filter(x => x.ParentLink.ID.Match(catalogId));
            var results = query.GetContentResult();

            var webCategories = new List<WebNavigation>();
            foreach (var hit in results)
            {
                if (hit == null) continue;
                var categoryItem = new WebNavigation()
                {
                    Name = hit.DisplayName,
                    Url = _urlResolver.GetUrl(hit.ContentLink),
                };
                GetSubcategoriesRecursive(hit, categoryItem);
                webCategories.Add(categoryItem);
            }

            return webCategories;
        }

        public WebNavigation GetSubcategoriesRecursive(WebCategory hit,
            WebNavigation categoryItem)
        {
            var query = _findClient
                .Search<WebCategory>().Filter(x => x.ParentLink.ID.Match(hit.ContentLink.ID));
            var results = query.GetContentResult();

            foreach (var innerHit in results)
            {
                if (innerHit == null) continue;
                var innerItem = new WebNavigation()
                {
                    Name = innerHit.DisplayName,
                    Url = _urlResolver.GetUrl(innerHit.ContentLink)
                };
                categoryItem.Child.Add(innerItem);
                var innerChild = _findClient
                    .Search<WebCategory>().Filter(x => x.ParentLink.ID.Match(innerHit.ContentLink.ID))
                    .GetContentResult();
                if (innerChild != null && innerChild.Any())
                    GetSubcategoriesRecursive(innerHit, innerItem);
            }

            return categoryItem;
        }
    }
}
