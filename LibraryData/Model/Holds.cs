﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData.Model
{
   public class Holds
    {

        public int Id { get; set; }
        public virtual LibraryAsset LibraryAsset { get; set; }
        public virtual LibraryCard LibraryCard { get; set; }
        public DateTime HoldPlaced { get; set; }
    }
}
