using EPiServer.Core;

namespace IDM.Shared.Models
{
    public class BookmarkModel
    {
        public ContentReference ContentLink { get; set; }
        public Guid ContentGuid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
