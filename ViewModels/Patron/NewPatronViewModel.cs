using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.Patron
{
    public class NewPatronViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(20, ErrorMessage ="Name Cannot Exceed 20 characters")]
        public string FirstName { get; set; }

        [Required, MaxLength(20, ErrorMessage = "Name Cannot Exceed 20 characters")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOFBirth { get; set; }
        public string Gender { get; set; }
        public string TelephoneNo { get; set; }

        //[Required]
        //[Display(Name = "CardId")]
        //public LibraryCard libraryCardId { get; set; }
        
        //[Required]
        //[Display(Name = "BranchId")]
        //public LibraryBranch BranchId { get; set; }

    }
}
