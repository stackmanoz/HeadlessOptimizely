namespace Headless.Features.CMS.Pages.StartPage
{
    [ContentType(
        GUID = "E695CA95-11C5-43FA-9E40-04EF6A145817",
        Order = 1,
        DisplayName = "Start Page",
        Description = "Start page of the application")]
    public class StartPage : PageData
    {
        [CultureSpecific]
        public virtual string Title { get; set; }
    }
}
