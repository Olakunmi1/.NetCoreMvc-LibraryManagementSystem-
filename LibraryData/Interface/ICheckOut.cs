using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public  interface ICheckOut
    {
        IEnumerable<Checkouts> GetAll();
        IEnumerable<Holds> GetCurrentHolds(int id); 
        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);

        Checkouts GetById(int id);
        Checkouts GetLatestCheckout(int id);

        int GetNumberOfCopies(int id); 
        string GetCurrentHoldPatron(int id);
        string GetCurrentHoldPlaced(int id);
        string GetCurrentPatron(int id);

        bool IsCheckedOut(int id);

        void Add(Checkouts newCheckout);
        void CheckoutItem(int id, int libraryCardId);
        void CheckInItem(int id);
        void PlaceHold(int assetId, int libraryCardId);
        void MarkLost(int id);
        void MarkFound(int id);
                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }
}
