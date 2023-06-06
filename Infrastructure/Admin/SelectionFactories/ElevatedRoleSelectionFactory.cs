using EPiServer.Shell.ObjectEditing;

namespace IDM.Infrastructure.Admin.SelectionFactories
{
    public class ElevatedRoleSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = "None"},
                new SelectItem { Text = "Reader", Value = "Reader"}
            };
        }
    }
}