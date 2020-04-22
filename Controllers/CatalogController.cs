using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData;
using LibraryManagement.Models.Catalog;
using LibraryManagement.ViewModels.Catalog;
using LibraryManagement.ViewModels.CheckOutModels;
using Microsoft.AspNetCore.Mvc;
using static LibraryManagement.ViewModels.Catalog.AssetDetailViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryManagement.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ILibraryAsset _assetServices;
        private readonly ICheckOut _checkoutsService;

        public CatalogController(ILibraryAsset assets, ICheckOut checkOut)
        {
            _assetServices = assets;
            _checkoutsService = checkOut;

        }
        // GET: /<controller>/
        
        public IActionResult Index()
        {
            var assetModel = _assetServices.GetAll();

            var listingResult = assetModel
                .Select(a => new AssetIndexListingViewModel
                {
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    AuthorOrDirector = _assetServices.GetAuthorOrDirector(a.Id),
                    Dewey = _assetServices.GetDeweyIndex(a.Id),
                    Title = _assetServices.GetTitle(a.Id),
                    Type = _assetServices.GetType(a.Id)
                }).ToList();

            var model = new AssetIndexViewModel
            {
                Assets = listingResult,
            };
            return View(model);
        }

        public IActionResult Detail(int Id)
        {
            var asset = _assetServices.GetById(Id);

            var currentHolds = _checkoutsService.GetCurrentHolds(Id)
            .Select(a => new AssetHoldModel
            {
                HoldPlaced = _checkoutsService.GetCurrentHoldPlaced(a.Id),
                PatronName = _checkoutsService.GetCurrentHoldPatron(a.Id)
            });

            var model = new AssetDetailViewModel
            {
                AssetId = Id,
                Title = asset.Title,
                Type = _assetServices.GetType(Id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assetServices.GetAuthorOrDirector(Id),
                //get current location(name) base on the ID   
                CurrentLocation = _assetServices.GetCurrentLocation(Id)?.Name, 
                DeweyCallNumber = _assetServices.GetDeweyIndex(Id),
                CheckoutHistory = _checkoutsService.GetCheckoutHistory(Id),
               // CurrentAssociatedLibraryCard = _assetServices.GetLibraryCardByAssetId(Id),
                ISBN = _assetServices.GetIsbn(Id),
                LatestCheckout = _checkoutsService.GetLatestCheckout(Id),
                CurrentHolds = currentHolds, 
                PatronName = _checkoutsService.GetCurrentPatron(Id)
            };
            return View(model);
        }

        //Serves the checkout page
        // CheckOut = to Borrow a book 
        [HttpGet]
        public  IActionResult Checkout(int Id)
        {
            var asset = _assetServices.GetById(Id);

            var model = new CheckoutViewModel
            {
                AssetId = Id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkoutsService.IsCheckedOut(Id)
               // HoldCount = _checkoutsService.GetCurrentHolds(Id).Count()
            };
            return View(model);
        }

        //Responds to Post method
        [HttpPost]
        public IActionResult CheckOut(int AssetId, int LibraryCardId)
        {
            _checkoutsService.CheckoutItem(AssetId, LibraryCardId);
           return RedirectToAction("Detail", new { id = AssetId });
           
        }

        //Responds to the checkIN operation
        // CheckIn = Return a book 
        public IActionResult CheckIn(int Id)
        {
            _checkoutsService.CheckInItem(Id);
            return RedirectToAction("Detail", new { Id });
        }

        //Also serves the Hold page 
        [HttpGet]
        public IActionResult Hold(int Id)
        {
            var asset = _assetServices.GetById(Id);

            var model = new CheckoutViewModel
            {
                AssetId = Id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkoutsService.IsCheckedOut(Id),
                HoldCount = _checkoutsService.GetCurrentHolds(Id).Count() 
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Hold(int AssetId, int LibraryCardId)
        {
            _checkoutsService.PlaceHold(AssetId, LibraryCardId);
            return RedirectToAction("Detail", new { id = AssetId });
        }

        public IActionResult MarkLost(int id)
        {
            _checkoutsService.MarkLost(id);
            return RedirectToAction("Detail", new { id });
        }

        public IActionResult MarkFound(int id)
        {
            _checkoutsService.MarkFound(id);
            return RedirectToAction("Detail", new { id });
        }

    }
}
