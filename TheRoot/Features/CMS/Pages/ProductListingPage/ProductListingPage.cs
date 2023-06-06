namespace IDM.Application.Features.CMS.Pages.ProductListingPage
{
    [ContentType(GUID = "02E1535E-CF3B-4C00-A92D-6D8EB218E860",
        DisplayName = "Product Listing Page",
        Description = "A listing page for products")]
    public class ProductListingPage : PageData
    {
        public virtual ContentArea ContentArea { get; set; }
    }
}
