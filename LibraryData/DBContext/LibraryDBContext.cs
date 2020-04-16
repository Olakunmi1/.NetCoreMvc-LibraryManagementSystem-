using LibraryData.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryData
{
    public class LibraryDBContext : DbContext
    {
        //public LibraryDBContext()
        //{

        //}
        public LibraryDBContext(DbContextOptions options)
            :base(options)
        {
        }

        //Series of DbSets that connects to the database
        public DbSet<Patron> Patrons { get; set; }
        public virtual DbSet<BranchHours> BranchHours { get; set; }
        public virtual DbSet<Book> Books { get; set; } 
        public virtual DbSet<Video> Videos { get; set; } 
        public virtual DbSet<Checkouts> Checkouts { get; set; } 
        public virtual DbSet<CheckoutHistory> CheckoutHistories { get; set; }
        public virtual DbSet<LibraryBranch> LibraryBranches { get; set; }
        public virtual DbSet<LibraryCard> LibraryCards { get; set; } 
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<LibraryAsset> LibraryAssets { get; set; } 
        public virtual DbSet<Holds> Holds { get; set; }


    }
}
