using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Reporting.Order;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Cms.Conventions;
using EPiServer.Find.Commerce;

namespace IDM.Application.Features.Find
{
    public class FindConventions : CatalogContentClientConventions
    {
        private readonly IClient _client;
        public FindConventions(FindCommerceOptions findCommerceOptions, IClient client) : base(findCommerceOptions)
        {
            _client = client;
        }

        public override void ApplyConventions(IClientConventions clientConventions)
        {
            base.ApplyConventions(clientConventions);
            ContentIndexer.Instance.Conventions.ForInstancesOf<ContentFolder>().ShouldIndex(x => false);
            ContentIndexer.Instance.Conventions.ForInstancesOf<SalesCampaignFolder>().ShouldIndex(x => false);
            ContentIndexer.Instance.Conventions.ForInstancesOf<CommerceReportingFolder>().ShouldIndex(x => false);
        }
    }
}
