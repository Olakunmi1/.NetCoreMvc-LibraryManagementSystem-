using LibraryData.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
   public interface ILibraryBranch
    {
        IEnumerable<LibraryBranch> GetAll();

        LibraryBranch Update(LibraryBranch branch);

        IEnumerable<Patron> GetPatrons(int patronid);

        IEnumerable<LibraryAsset> GetAssets(int branchId);
        LibraryBranch GetById(int branchId);
        void Add(LibraryBranch newBranch);
        IEnumerable<string> GetBranchHours(int branchId);
        bool IsBranchOpen(int branchId);
        int GetAssetCount(int branchId);
        int GetPatronCount(int branchId);  
        decimal GetAssetsValue(int id);

    }
}
