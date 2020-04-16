using LibraryData;
using LibraryData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
   public class PatronService : IPatron
    {
        private readonly LibraryDBContext _context;

        public PatronService(LibraryDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Patron> GetAll()
        {
            //Eager loading the data
            //with Key word "Include"
            return _context.Patrons
                 .Include(a => a.HomeLibraryBranch)
                 .Include(a => a.LibraryCard);
        }

        public Patron GetById(int id)
        {
            return _context.Patrons
                 .Include(p => p.HomeLibraryBranch)
                 .Include(p => p.LibraryCard)
                 .FirstOrDefault(p => p.Id == id);
        }

        public void Add(Patron newpatron)
        {
            _context.Add(newpatron);
            _context.SaveChanges();
        }

        public IEnumerable<CheckoutHistory> GetCheckOutHistory(int patronId)
        {
            var cardId = _context.Patrons
               .Include(a => a.LibraryCard)
               .FirstOrDefault(a => a.Id == patronId)?
               .LibraryCard.Id;

            return _context.CheckoutHistories
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.CheckedOut);
        }

        public IEnumerable<Checkouts> GetCheckOuts(int id)
        {

            var patronCardId = GetById(id).LibraryCard.Id; 

            return _context.Checkouts
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == patronCardId);
        }

        public IEnumerable<Holds> GetHolds(int patronId)
        {
            //get the library card
            var cardId = _context.Patrons
                .Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.Id == patronId)?
                .LibraryCard.Id;

            return _context.Holds
                 .Include(h => h.LibraryAsset)
                 .Include(h => h.LibraryCard)
                 .Where(a => a.LibraryCard.Id == cardId)
                 .OrderByDescending(a => a.HoldPlaced);

                
        }

    }
}
