using EPiServer.DataAnnotations;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;

namespace IDM.Infrastructure.Admin.Media
{
    [ContentType(DisplayName = "Vector Image File", GUID = "F522B459-EB27-462C-B216-989FC7FF9448")]
    [MediaDescriptor(ExtensionString = "svg")]
    public class VectorImageMediaData : ImageMediaData
    {
        public override Blob Thumbnail => BinaryData;
        public override Blob LargeThumbnail => BinaryData;
    }
}
