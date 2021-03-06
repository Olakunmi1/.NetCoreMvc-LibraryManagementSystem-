﻿using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.ViewModels.Patron
{
    public class PatronDetailViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? LibraryCardId { get; set; }
        public string Address { get; set; }
        public DateTime? MemberSince { get; set; }
        public string Telephone { get; set; }
        public string HomeLibrary { get; set; }
        public decimal? Overduess { get; set; }
        public IEnumerable<Checkouts> AssetCheckedOut { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
        public IEnumerable<Holds> Holds { get; set; }


    }
}
