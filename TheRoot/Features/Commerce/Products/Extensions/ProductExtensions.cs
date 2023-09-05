using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using IDM.Application.Features.Commerce.Category;
using Mediachase.Commerce.Catalog;

namespace IDM.Application.Features.Commerce.Products.Extensions
{
    public static class ProductExtensions
    {
        #region Members

        private static Injected<IContentLoader> _contentLoader;
        private static Injected<UrlResolver> _urlResolver;

        #endregion

        #region Categories

        public static string Url(this NodeContent content)
        {
            return _urlResolver.Service.GetUrl(content.ContentLink, content.Language.Name);
        }

        public static IEnumerable<string> AllCategoryIds(this ProductContent content)
        {
            return content.GetCategories().Select(x => x.ToReferenceWithoutVersion().ToString());
        }

        public static IEnumerable<string> AllParentCategoryNames(this ProductContent content)
        {
            var categories = content.GetCategories();
            var result = new List<string>();

            foreach (var cat in categories)
            {
                var currentNode = _contentLoader.Service.Get<NodeContent>(cat);

                if (currentNode != null)
                {
                    result.Add($"{currentNode.DisplayName}|{currentNode.Url()}");
                }

                var parentLinks = _contentLoader.Service.GetAncestors(cat);

                foreach (var link in parentLinks)
                {
                    if (link is NodeContent node)
                    {
                        result.Add($"{node.DisplayName}|{node.Url()}");
                    }
                }
            }

            return result;
        }

        public static IEnumerable<string> AllParentCategoryIds(this ProductContent content)
        {
            var categories = content.GetCategories();
            var result = new List<string>();

            foreach (var cat in categories)
            {
                result.Add(cat.ToReferenceWithoutVersion().ToString());

                var parentLinks = _contentLoader.Service.GetAncestors(cat);

                result.AddRange(parentLinks.OfType<NodeContent>().Select(link => link.ContentLink.ToReferenceWithoutVersion().ToString()));
            }

            return result;
        }
        #endregion
    }
}
