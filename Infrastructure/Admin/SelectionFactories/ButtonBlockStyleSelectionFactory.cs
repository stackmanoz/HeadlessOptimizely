using EPiServer.Shell.ObjectEditing;
using IDM.Shared.Contants;

namespace IDM.Infrastructure.Admin.SelectionFactories
{
    public class ButtonBlockStyleSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Transparent Black", Value = IDMConstants.ButtonBlockStyles.TransparentBlack },
                new SelectItem { Text = "Transparent White", Value = IDMConstants.ButtonBlockStyles.TransparentWhite },
                new SelectItem { Text = "Dark", Value = IDMConstants.ButtonBlockStyles.Dark },
                new SelectItem { Text = "White", Value = IDMConstants.ButtonBlockStyles.White },
                new SelectItem { Text = "Yellow Black", Value = IDMConstants.ButtonBlockStyles.YellowBlack },
                new SelectItem { Text = "Yellow White", Value = IDMConstants.ButtonBlockStyles.YellowWhite }
            };
        }
    }
}
