using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.Branch
{
    public class EditBranchViewModel : NewBranchViewModel
    {
        //public int Id { get; set; }

        [Required]
        public string ExistingImageName { get; set; }

    }
}
