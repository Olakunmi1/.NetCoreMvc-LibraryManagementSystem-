using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.Branch
{
    public class branchIndexModel
    {
        public IEnumerable<branchDetailViewModel> Branches { get; set; }
    }
}
