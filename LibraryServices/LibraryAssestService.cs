using LibraryData;
using LibraryData.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    //This class implements IlibraryAssetService
    public class LibraryAssestService : ILibraryAsset
    {
        private readonly LibraryDBContext context;

        //Creating and initializing DbContext
        public LibraryAssestService(LibraryDBContext context)
        {
            this.context = context;
        }

        public void Add(LibraryAsset newAsset)
        {
           // context.LibraryAssets.Add(newAsset);
            context.Add(newAsset);
            context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            //The "Include" method returns the navigation prop
            //Eager loading the prop with LibraryAsset...
            return context.LibraryAssets
              .Include(a => a.Status)
              .Include(a => a.Location);
        }

        public LibraryAsset GetById(int id)
        {
            //var Asset = GetAll().FirstOrDefault(a => a.Id == id);
            //return Asset;
            return context.LibraryAssets
                     .Include(a => a.Status)
                     .Include(a => a.Location)
                     .FirstOrDefault(a => a.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
           var currentLocation = context.LibraryAssets.First(a => a.Id == id).Location;
            return currentLocation;
        }

        public string GetDeweyIndex(int id)
        {
           if(context.Books.Any(book => book.Id == id))
            {
                return context.Books.FirstOrDefault(book => book.Id == id).DeweyIndex;
            }

            return "";
        }

        public string GetIsbn(int id)
        {
            if (context.Books.Any(book => book.Id == id))
            {
                return context.Books.FirstOrDefault(book => book.Id == id).ISBN;
            }

            return "";
        }

        public string GetTitle(int id)
        {
            return context.LibraryAssets.FirstOrDefault(a => a.Id == id).Title;
        }

        public string GetType(int id) 
        {
            // Hack
            //Checking the libraryAsest for a type of "Book"
            var book = context.LibraryAssets
                .OfType<Book>().SingleOrDefault(a => a.Id == id);

            //if the book parameter above is not empty, return "Book", esle "video"
            return book != null ? "Book" : "Video";
        }

        public string GetAuthorOrDirector(int id)
        {
            //check if it is of type book 
            var IsBook = context.LibraryAssets.OfType<Book>()
                .Where(asset => asset.Id == id).Any();

            //check if its of type Video
            var IsVideo = context.LibraryAssets.OfType<Video>()
                .Where(asset => asset.Id == id).Any();

            //if IsBook returned True,get the Author, else get the director 4rm Video 
            //otherwise return the string "Únknown"..
            return IsBook ?
                context.Books.FirstOrDefault(b => b.Id == id).Author :
                context.Videos.FirstOrDefault(video => video.Id == id).Director
                ?? "Unknown";
        }

    }
}
