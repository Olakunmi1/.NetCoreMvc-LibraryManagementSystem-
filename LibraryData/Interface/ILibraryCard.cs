using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData.Interface
{
    public interface ILibraryCard
    {
        IEnumerable<LibraryCard> GetAll();
        void Add(LibraryCard libraryCard);
        LibraryCard GetById(int id);
    }
}
