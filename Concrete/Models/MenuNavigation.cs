namespace Concrete.Models
{
    public class CustomerNavigationItem
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public List<CustomerNavigationItem> Child { get; set; } = new();
    }
}
