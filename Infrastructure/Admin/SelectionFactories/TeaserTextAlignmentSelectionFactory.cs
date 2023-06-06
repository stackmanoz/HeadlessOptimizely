using EPiServer.Shell.ObjectEditing;

namespace IDM.Infrastructure.Admin.SelectionFactories
{
    public class TeaserTextAlignmentSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Left", Value = "Left" },
                new SelectItem { Text = "Right", Value = "Right" },
                new SelectItem { Text = "Center", Value = "Center" },
            };
        }
    }
}
