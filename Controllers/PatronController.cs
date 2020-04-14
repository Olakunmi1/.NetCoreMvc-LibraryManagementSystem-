using LibraryData;
using LibraryData.Interface;
using LibraryData.Model;
using LibraryManagement.ViewModels.Patron;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManagement.Controllers
{
    public class PatronController : Controller
    {
        private readonly IPatron _patron;
       
        public PatronController(IPatron patron)
        {
           _patron = patron;
        }

        //This serves the NewPatron View
        [HttpGet]
        public IActionResult NewPatron()
        {
            return View();
        }

        //This responds to Post Method for NewPatron
        [HttpPost]
        public IActionResult NewPatron(NewPatronViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newPatron = new Patron
                {
                    FirstName = model.FirstName,
                    Lastname = model.LastName,
                    Address = model.Address,
                    DateOfBirth = model.DateOFBirth,
                    Telephone = model.TelephoneNo,
                    Gender = model.Gender,
                    //HomeLibraryBranch = model.BranchId,
                    //LibraryCard = model.libraryCardId
                };
                _patron.Add(newPatron);
                return RedirectToAction("Index");
                
            }

            return View();
        }

        public IActionResult Index()
        {
            var allPatrons = _patron.GetAll();

            var patronModels = allPatrons
                .Select(p => new PatronDetailViewModel
                {
                     Id = p.Id,
                     LastName = p.Lastname ?? "No Last Name Provided",
                     FirstName = p.FirstName ?? "No First Name Provided",
                     LibraryCardId = p.LibraryCard?.Id,
                     Overduess = p.LibraryCard?.Fees,
                     HomeLibrary = p.HomeLibraryBranch?.Name
                }).ToList();

            var model = new PatronIndexViewModel
            {
                patrons = patronModels
            };

            return View(model);

        }

        public IActionResult Detail(int id)
        {
            var patron = _patron.GetById(id);

            var model = new PatronDetailViewModel
            {
                Id = patron.Id,
                //If the lastname wasnt given, pass in the string value just beside the double query string
                LastName = patron.Lastname ?? "No Last Name Provided", 
                FirstName = patron.FirstName ?? "No First Name Provided",
                Address = patron.Address ?? "No Address Provided",
                HomeLibrary = patron.HomeLibraryBranch?.Name ?? "No Home Library",
                MemberSince = patron.LibraryCard?.Created,
                Overduess = patron.LibraryCard?.Fees,
                LibraryCardId = patron.LibraryCard?.Id,
                Telephone = string.IsNullOrEmpty(patron.Telephone) ? "No Telephone Number Provided" : patron.Telephone,
                AssetCheckedOut = _patron.GetCheckOuts(id).ToList(),
                CheckoutHistory = _patron.GetCheckOutHistory(id),
                Holds = _patron.GetHolds(id)
            };

            return View(model);
        }


    }
}
