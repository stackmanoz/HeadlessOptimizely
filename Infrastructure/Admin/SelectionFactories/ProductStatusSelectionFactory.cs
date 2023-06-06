﻿using EPiServer.Shell.ObjectEditing;

namespace IDM.Infrastructure.Admin.SelectionFactories
{
    public class ProductStatusSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Active", Value = "Active" },
                new SelectItem { Text = "Inactive", Value = "Inactive" },
                new SelectItem { Text = "Discontinued", Value = "Discontinued" }
            };
        }
    }
}