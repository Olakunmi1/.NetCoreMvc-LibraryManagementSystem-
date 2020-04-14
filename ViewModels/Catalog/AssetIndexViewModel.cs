using LibraryManagement.Models.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.Catalog
{
    public class AssetIndexViewModel
    {
        public IEnumerable<AssetIndexListingViewModel> Assets { get; set; }
    }
}
