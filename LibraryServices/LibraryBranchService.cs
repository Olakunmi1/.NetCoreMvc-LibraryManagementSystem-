using LibraryData;
using LibraryData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
   public class LibraryBranchService : ILibraryBranch
    {
        private LibraryDBContext _context;
        public LibraryBranchService(LibraryDBContext context)
        {
            _context = context;
        }
        public void Add(LibraryBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return _context.LibraryBranches
                 .Include(b => b.LibraryAssets)
                 .Include(b => b.Patrons);
        }

        public LibraryBranch GetById(int branchId) 
        {
            return _context.LibraryBranches
                .Include(b => b.LibraryAssets)
                .Include(b => b.Patrons)
                .FirstOrDefault(b => b.Id == branchId);
        }

        public IEnumerable<LibraryAsset> GetAssets(int branchId)
        {
            var result = _context.LibraryBranches
                 .Include(b => b.LibraryAssets)
                 .FirstOrDefault(b => b.Id == branchId).LibraryAssets;
            return result;
        }
        public IEnumerable<Patron> GetPatrons(int patronid)
        {
            return _context.LibraryBranches
                .Include(b => b.Patrons)
                .FirstOrDefault(b => b.Id == patronid).Patrons;
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours.Where(h => h.Branch.Id == branchId);

            var displayHours =
                DataHelpers.HumanizeBusinessHours(hours);

            return displayHours;
        }
        public bool IsBranchOpen(int branchId)
        {
            return true;
        }
        public int GetAssetCount(int branchId)
        {
            return GetById(branchId).LibraryAssets
                .Count();
        }

        public decimal GetAssetsValue(int id)
        {
            var assetsValue = GetAssets(id).Select(a => a.Cost);
            return assetsValue.Sum();
        }

        public int GetPatronCount(int branchId) 
        {
            return GetById(branchId).Patrons
                .Count();
        }

        public LibraryBranch Update(LibraryBranch branch)
        {
            //here we are attaching the new change with "Attach" 
            //keyword, inorder to track chnges 
            var branchChanges = _context.LibraryBranches.Attach(branch);
            if(branchChanges != null)
            {
                //and we tell entity framework that the state of the object coming is modified
                branchChanges.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            _context.SaveChanges();
            return branch;
        }
    }
}
