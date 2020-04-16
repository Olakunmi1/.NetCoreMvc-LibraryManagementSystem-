using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
   public interface IPatron
    {
        IEnumerable<Patron> GetAll();
        Patron GetById(int id);

        void Add(Patron newpatron);
        IEnumerable<CheckoutHistory> GetCheckOutHistory(int patronId);
        IEnumerable<Holds> GetHolds(int patronId);
        IEnumerable<Checkouts> GetCheckOuts(int id);


    }
}
