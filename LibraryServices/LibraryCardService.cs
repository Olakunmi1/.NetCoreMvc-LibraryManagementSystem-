using LibraryData;
using LibraryData.Interface;
using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class LibraryCardService : ILibraryCard
    {
        private readonly LibraryDBContext _context;

        public LibraryCardService(LibraryDBContext context)
        {
            _context = context;
        }
        public void Add(LibraryCard libraryCard)
        {
            _context.Add(libraryCard);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryCard> GetAll()
        {
            return _context.LibraryCards;
        }

        public LibraryCard GetById(int id)
        {
            var card = _context.LibraryCards
                 .FirstOrDefault(c => c.Id == id);
            return card; 
        }
    }
}
