using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Models.Catalog
{
    public class AssetIndexListingViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; }
        public string Type { get; set; }
        public string Dewey { get; set; }
        public int NumberOfCopies { get; set; }
        public int CopiesAvailable { get; set; } 
    }
}
