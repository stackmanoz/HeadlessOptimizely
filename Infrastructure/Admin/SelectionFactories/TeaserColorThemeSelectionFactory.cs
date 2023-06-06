using EPiServer.Shell.ObjectEditing;

namespace IDM.Infrastructure.Admin.SelectionFactories
{
    public class TeaserColorThemeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Light", Value = "Light" },
                new SelectItem { Text = "Dark", Value = "Dark" }
            };
        }
    }
}
