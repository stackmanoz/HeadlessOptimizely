using EPiServer.Shell.ObjectEditing;

namespace IDM.Infrastructure.Admin.SelectionFactories
{
    public class VirtualVariantTypeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = "None" },
                new SelectItem { Text = "Key", Value = "Key" },
                new SelectItem { Text = "File", Value = "File" },
                new SelectItem { Text = "Elevated Role", Value = "ElevatedRole" }
            };
        }
    }
}