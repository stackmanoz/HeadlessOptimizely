namespace IDM.Shared.Models
{
    public class WebNavigation
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public List<WebNavigation> Child { get; set; } = new();
    }
}
