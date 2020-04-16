﻿using LibraryData;
using LibraryData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LibraryServices
{
   public class CheckOutServices : ICheckOut
    {
        private readonly LibraryDBContext _context;

        //Injecting the database
        public CheckOutServices(LibraryDBContext context)
        {
            _context = context;
        }
        public void Add(Checkouts newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkouts> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkouts GetById(int id)
        {
            return _context.Checkouts.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            var  histories =  _context.CheckoutHistories
               .Include(a => a.LibraryAsset)
               .Include(a => a.LibraryCard)
               .Where(a => a.LibraryAsset.Id == id);
            return histories;
        }

        public string GetCurrentHoldPatron(int id) 
        {
            var hold = _context.Holds
                 .Include(a => a.LibraryAsset)
                 .Include(a => a.LibraryCard)
                 .Where(v => v.Id == id);

            var cardId = hold
                .Include(a => a.LibraryCard)
                .Select(a => a.LibraryCard.Id)
                .FirstOrDefault();

            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.Lastname;
        }

        public IEnumerable<Holds> GetCurrentHolds(int id)
        {
            return _context.Holds
                 .Include(a => a.LibraryCard)
                 .Where(h => h.LibraryAsset.Id==id);
        }

        public string GetCurrentPatron(int id)
        {
            var checkout = _context.Checkouts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);

            if (checkout == null)
                return "Not checked out.";

            var cardId = checkout.LibraryCard.Id;

            var patron = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(c => c.LibraryCard.Id == cardId);
            if (patron == null)
                return "Not found";

            return patron.FirstName + " " + patron.Lastname;
        }

        public Checkouts GetLatestCheckout(int id)
        {
            return _context.Checkouts
                .Where(c => c.LibraryAsset.Id == id)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault(); 
        }

        public void MarkLost(int id)
        {
            var item = _context.LibraryAssets
               .FirstOrDefault(a => a.Id == id);

            //calling the Update method, will enable EfCore to track the changes
            _context.Update(item);

            item.Status = _context.Statuses.FirstOrDefault(a => a.Name == "Lost");

            _context.SaveChanges();
        }

        public void MarkFound(int id)
        {
            var now = DateTime.Now;
            var item = _context.LibraryAssets
               .FirstOrDefault(a => a.Id == id);

            //calling the Update method, will enable EfCore to track the changes
            _context.Update(item);

            item.Status = _context.Statuses.FirstOrDefault(a => a.Name == "Available");

            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkout != null) 
                _context.Remove(checkout);

            // close any existing checkout history
            var history = _context.CheckoutHistories
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }

            _context.SaveChanges();

        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var asset = _context.LibraryAssets
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            //checking if the CardId Exist 
            var card = _context.LibraryCards
                .FirstOrDefault(a => a.Id == libraryCardId);

            _context.Update(asset);

            if (asset.Status.Name == "Available")
                asset.Status = _context.Statuses.FirstOrDefault(a => a.Name == "On Hold");

            var hold = new Holds
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _context.Add(hold);
            _context.SaveChanges();
        }

        public void CheckInItem(int id)
        {
            var now = DateTime.Now;

            var item = _context.LibraryAssets
                .FirstOrDefault(a => a.Id == id); 

            //Setting it up for an update...
            _context.Update(item);

            // remove any existing checkouts on the item
            var checkout = _context.Checkouts
                .Include(c => c.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkout != null) 
                _context.Remove(checkout);

            //  existing checkout history
            var history = _context.CheckoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                _context.Update(history);
                history.CheckedIn = now;
            }

            // look for current holds
            var currentHolds = _context.Holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);

            // if there are current holds, check out the item to the earliest
            if (currentHolds.Any())
            {
                CheckoutToEarliestHold(id, currentHolds);
                return;
            }

            // otherwise, set item status to available
            item.Status = _context.Statuses.FirstOrDefault(a => a.Name == "Available");

            _context.SaveChanges();
        }

        //A private method used within this CheckOutService
        private void CheckoutToEarliestHold(int id, IQueryable<Holds> currentHolds)
        {
            var earliestHold = currentHolds.OrderBy(a => a.HoldPlaced).FirstOrDefault();
            if (earliestHold == null) 
                return;
            var card = earliestHold.LibraryCard;
            _context.Remove(earliestHold);
            _context.SaveChanges();

            //checking out the item
            CheckoutItem(id, card.Id);
        }

        //checking out item 
        public void CheckoutItem(int id, int libraryCardId)
        {
            if (IsCheckedOut(id)) 
                return ;

            var item = _context.LibraryAssets
                .Include(a => a.Status)
                .FirstOrDefault(a => a.Id == id);

            //setting it up for update 
            _context.Update(item);

            item.Status = _context.Statuses
                .FirstOrDefault(a => a.Name == "Checked Out");

            var now = DateTime.Now;

            var libraryCard = _context.LibraryCards
                .Include(c => c.Checkouts)
                .FirstOrDefault(a => a.Id == libraryCardId);

            var checkout = new Checkouts
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };

            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public int GetNumberOfCopies(int id)
        {
            return _context.LibraryAssets
               .FirstOrDefault(a => a.Id == id)
               .NumberOfCopies;
        }

        public bool IsCheckedOut(int id)
        {
            var isCheckedOut = _context.Checkouts.Any(a => a.LibraryAsset.Id == id);

            return isCheckedOut;
        }

        public string GetCurrentHoldPlaced(int id)
        {
            var hold = _context.Holds
                 .Include(a => a.LibraryAsset)
                 .Include(a => a.LibraryCard)
                 .Where(v => v.Id == id);

            return hold.Select(a => a.HoldPlaced)
                .FirstOrDefault().ToString(CultureInfo.InvariantCulture);
        }
    }

}
