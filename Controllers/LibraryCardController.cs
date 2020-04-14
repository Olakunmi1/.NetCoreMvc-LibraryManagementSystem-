using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryData.Interface;
using LibraryData.Model;
using LibraryManagement.ViewModels.LibraryCad;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LibraryManagement.Controllers
{
    public class LibraryCardController : Controller
    {
        private readonly ILibraryCard _CardServices;

        public LibraryCardController(ILibraryCard card)
        {
            _CardServices = card;
        }
        public IActionResult Index()
        {
            var AllCards = _CardServices.GetAll();
            var model = AllCards
                .Select(card => new LibraryCardViewModel
                {
                     Id = card.Id,
                     Fees = card.Fees,
                     TimeCreated = card.Created
                });

            return View(model);
        }

       //Serves the LibraryCard Page
        [HttpGet]
        public IActionResult CreateLibraryCard()
        {
            return View();
        }

        //responds to post request 
        [HttpPost]
        public IActionResult CreateLibraryCard(LibraryCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCard = new LibraryCard
                {
                    Id = model.Id,
                    Created = model.TimeCreated,
                    Fees = model.Fees
                };
                _CardServices.Add(newCard);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
