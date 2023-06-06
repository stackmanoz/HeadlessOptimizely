using EPiServer.Cms.Shell.UI.Rest.Models;
using EPiServer.Cms.Shell.UI.Rest.Models.Transforms;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.ServiceLocation;

namespace IDM.Application.Features.Commerce.Transformers
{
    [ServiceConfiguration(typeof(IModelTransform), Lifecycle = ServiceInstanceScope.Singleton)]
    public class NodeContentModelTransform : TransformBase<ContentDataStoreModelBase>
    {
        public override TransformOrder Order => TransformOrder.TransformEnd;

        protected override bool ShouldTransformInstance(IModelTransformContext context)
        {
            return context.Source is NodeContent;
        }

        public override void TransformInstance(IContent source, ContentDataStoreModelBase target,
            IModelTransformContext context)
        {
            var node = source as NodeContent;

            if (node == null)
            {
                return;
            }

            // Set the public URL as required
            target.PublicUrl = node.SeoUri;
        }
    }
}
