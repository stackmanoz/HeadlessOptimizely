using EPiServer.Find;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Commerce;
using IDM.Application.Features.Commerce.Products;
using IDM.Application.Services.Product;

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

            clientConventions.TypeValidationConventions = new TypeValidationConventions();

            #region catalogConventions
            clientConventions.ForInstancesOf<GenericProduct>().ApplyProductConventions();
            #endregion
        }
    }
}
