using IDM.Shared.Models.Address;

namespace IDM.Application.Features.CMS.Pages.AddressBook
{
    public class AddressCollectionViewModel
    {
        public AddressCollectionViewModel(IEnumerable<AddressModel> addresses)
        {
            Addresses = addresses;
        }

        public AddressCollectionViewModel(AddressBookPage currentPage, IEnumerable<AddressModel> addresses)
        {
            Addresses = addresses;
        }

        public IEnumerable<AddressModel> Addresses { get; set; }
    }
}
