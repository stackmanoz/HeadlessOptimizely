using IDM.Shared.Models;

namespace IDM.Application.Services.ContentModel
{
    public interface IContentViewModel<out TContent> where TContent : class
    {
        List<WebNavigation> CmsNavigation { get; }
        List<WebNavigation> CommerceNavigation { get; }
        TContent CurrentContent { get; }
    }
}
