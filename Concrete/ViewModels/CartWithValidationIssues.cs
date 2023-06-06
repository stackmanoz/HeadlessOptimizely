using EPiServer.Commerce.Order;

namespace IDM.Shared.ViewModels
{
    public class CartWithValidationIssues
    {
        public virtual ICart Cart { get; set; }
        public virtual Dictionary<ILineItem, List<ValidationIssue>> ValidationIssues { get; set; }
    }
}
