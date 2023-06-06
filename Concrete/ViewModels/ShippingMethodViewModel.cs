using Mediachase.Commerce;

namespace IDM.Shared.ViewModels
{
    public class ShippingMethodViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Money Price { get; set; }
        public bool IsInstorePickup { get; set; }
    }
}