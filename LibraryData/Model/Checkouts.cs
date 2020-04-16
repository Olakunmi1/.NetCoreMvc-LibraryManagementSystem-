﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryData.Model
{
   public class Checkouts
    {
        public int Id { get; set; } 

        [Required]
        public LibraryAsset LibraryAsset { get; set; }

        public LibraryCard LibraryCard { get; set; }

        public DateTime Since { get; set; }

        public DateTime Until { get; set; }

    }
}