using EPiServer.Commerce.Order;
using IDM.Application.Features.Commerce;
using IDM.Shared.Models.Address;
using IDM.Shared.ViewModels;
using Mediachase.Commerce.Customers;
using CountryViewModel = EPiServer.Commerce.UI.Admin.Markets.Internal.CountryViewModel;

namespace IDM.Application.Features.CMS.Pages.AddressBook
{
    public interface IAddressBookService
    {
        AddressCollectionViewModel GetAddressBookViewModel(AddressBookPage addressBookPage);
        IList<AddressModel> List();
        bool CanSave(AddressModel addressModel);
        void Save(AddressModel addressModel, CommerceContact contact = null);
        void Delete(string addressId);
        void SetPreferredBillingAddress(string addressId);
        void SetPreferredShippingAddress(string addressId);
        CustomerAddress GetPreferredBillingAddress();
        void LoadAddress(AddressModel addressModel);
        void LoadCountriesAndRegionsForAddress(AddressModel addressModel);
        IEnumerable<string> GetRegionsByCountryCode(string countryCode);
        void MapToAddress(AddressModel addressModel, IOrderAddress orderAddress);
        void MapToAddress(AddressModel addressModel, CustomerAddress customerAddress);
        void MapToModel(CustomerAddress customerAddress, AddressModel addressModel);
        void MapToModel(IOrderAddress orderAddress, AddressModel addressModel);
        IOrderAddress ConvertToAddress(AddressModel addressModel, IOrderGroup orderGroup);
        AddressModel ConvertToModel(IOrderAddress orderAddress);

        IList<AddressModel> MergeAnonymousShippingAddresses(IList<AddressModel> addresses,
            IEnumerable<CartItemViewModel> cartItems);

        bool UseBillingAddressForShipment();
        IEnumerable<CountryViewModel> GetAllCountries();
        string GetCountryNameByCode(string code);
        void DeleteAddress(string organizationId, string addressId);
        AddressModel GetAddress(string addressId);
        AddressModel ConvertAddress(CustomerAddress customerAddress);
    }
}
