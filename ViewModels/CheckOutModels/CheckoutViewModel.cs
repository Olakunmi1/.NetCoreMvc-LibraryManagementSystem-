using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.CheckOutModels
{
    public class CheckoutViewModel
    {
        public int AssetId { get; set; }
        public string LibraryCardId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public int HoldCount { get; set; }
        public bool IsCheckedOut { get; set; }
    }
}
