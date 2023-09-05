using EPiServer.Commerce.SpecializedProperties;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace IDM.Infrastructure.Admin.Media;

[ContentType(DisplayName = "Image Media Data", GUID = "ccbe8d9b-5eb8-4743-b630-f1b3a93977c7",
    Description = "Image media data")]
[MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png,svg")]
public class ImageMediaData : CommerceImage
{
    [Display(
        Name = "Width",
        Description = "The width of the image.",
        Order = 40)]
    [Editable(false)]
    public virtual int? Width { get; set; }

    [Display(
        Name = "Height",
        Description = "The height of the image.",
        Order = 50)]
    [Editable(false)]
    public virtual int? Height { get; set; }
}