using FluentAssertions;
using LibraryData;
using LibraryData.Model;
using LibraryManagement.Controllers;
using LibraryManagement.ViewModels.Patron;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LibraryManagement.Test.Controller
{
    
   public class PatronContollerTest
    {
        private static IEnumerable<Patron> GetAllPatron()
        {
            List<Patron> allPatron = new List<Patron>
           {
               new Patron
               {
                    Id = 22,
                    FirstName = "Adebowale",
                    Address = "3, cole johnson",
                    Telephone = "234",
               },

               new Patron
               {
                    Id = 23,
                    FirstName = "Badmos",
                    Address = "4, coleson johnson",
                    Telephone = "999",
               }

           };
            return allPatron;
        }

        private static Patron GetPatron()
        {
            var patron = new Patron
            {
                Id = 24,
                FirstName = "Badmos",
                Lastname = "Barkley",
                Address = "4, coleson johnson",
                Telephone = "999",

                LibraryCard = new LibraryCard
                {
                    Id = 123,
                    Created = new DateTime(2018, 2, 12)

                },
                HomeLibraryBranch = new LibraryBranch
                {
                    Id = 12,
                    Name = "Stephen Hawkins",

                }

            };
            return patron;
        }

        private static Patron GetNoInfoPatron()
        {
            return new Patron();
        }

        [Fact]
        public void Should_Return_List_Of_Patron()
        {
            //Arrange
            var mockpatron = new Mock<IPatron>();
            mockpatron.Setup(x => x.GetAll()).Returns(GetAllPatron());
            var controller = new PatronController(mockpatron.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewmodel = viewResult.Subject.Model.Should().BeAssignableTo<PatronIndexViewModel>();

            viewmodel.Subject.patrons.Count().Should().Be(2);

            //Verifying if this query was ran when GetAll Method was called
            mockpatron.Verify(x => x.GetAll(), Times.Once());

        }

        [Fact]
        public void Should_Return_Patrondetails_When_Detail_Called()
        {
            var mockPatron = new Mock<IPatron>();
            mockPatron.Setup(r => r.GetById(24)).Returns(GetPatron());
            mockPatron.Setup(r => r.GetCheckOuts(1)).Returns(new List<Checkouts> { });
            mockPatron.Setup(r => r.GetCheckOutHistory(1)).Returns(new List<CheckoutHistory> { });
            mockPatron.Setup(r => r.GetHolds(1)).Returns(new List<Holds> { });
            var sut = new PatronController(mockPatron.Object);

            var result = sut.Detail(24);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<PatronDetailViewModel>();

            viewModel.Subject.FirstName.Should().Be("Badmos");
            viewModel.Subject.LastName.Should().Be("Barkley");
            viewModel.Subject.LibraryCardId.Should().Be(123);
            viewModel.Subject.HomeLibrary.Should().Be("Stephen Hawkins");


            //Verifying if this queries were ran when called
            mockPatron.Verify(x => x.GetCheckOuts(24), Times.Once());
            mockPatron.Verify(x => x.GetCheckOutHistory(24), Times.Once());
            mockPatron.Verify(x => x.GetHolds(24), Times.Once());

        }

        [Fact]
        public void Should_Return_Default_Values_For_Missing_Patron_Details_When_Detail_Called()
        {
            var mockPatronService = new Mock<IPatron>();
            mockPatronService.Setup(r => r.GetById(4)).Returns(GetNoInfoPatron());
            var controller = new PatronController(mockPatronService.Object);

            var result = controller.Detail(4);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<PatronDetailViewModel>();
            viewModel.Subject.FirstName.Should().Be("No First Name Provided");
            viewModel.Subject.LastName.Should().Be("No Last Name Provided");
            viewModel.Subject.Address.Should().Be("No Address Provided");
            viewModel.Subject.HomeLibrary.Should().Be("No Home Library");
            viewModel.Subject.Telephone.Should().Be("No Telephone Number Provided");
        }


      
    }
}
