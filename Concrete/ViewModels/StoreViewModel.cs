namespace IDM.Shared.ViewModels
{
    public class StoreViewModel
    {
        public bool ShowDelivery { get; set; } = true;
        public IList<StoreItemViewModel> Stores { get; set; }
        public string SelectedStore { get; set; }
        public string SelectedStoreName { get; set; }
    }
}
