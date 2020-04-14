using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.LibraryCad
{
    public class LibraryCardViewModel
    {
        public int Id { get; set; }

        [Required]
        public decimal Fees { get; set; }

        [Required]
        public DateTime TimeCreated { get; set; }
    }
}
