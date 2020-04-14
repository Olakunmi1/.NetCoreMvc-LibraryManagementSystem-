using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.Branch
{
    public class NewBranchViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Telephone { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime OpenDate { get; set; }

        [Required]
        public IFormFile ImageUrl { get; set; }
    }
}
