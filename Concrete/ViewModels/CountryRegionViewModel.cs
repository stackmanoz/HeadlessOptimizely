using IDM.Shared.Attributes;

namespace IDM.Shared.ViewModels
{
    public class CountryRegionViewModel
    {
        public IEnumerable<string> RegionOptions { get; set; }

        [LocalizedDisplay("/Shared/Address/Form/Label/CountryRegion")]
        public string Region { get; set; }

        public string SelectClass { get; set; } = "select";
        public string TextboxClass { get; set; } = "textbox";
    }
}
