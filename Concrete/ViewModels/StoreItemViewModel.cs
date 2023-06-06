namespace IDM.Shared.ViewModels
{
    public class StoreItemViewModel
    {
        public string Code { get; set; }
        public bool IsFulfillmentCenter { get; set; }
        public bool IsPickupLocation { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string Name { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public decimal Inventory { get; set; }
    }
}
