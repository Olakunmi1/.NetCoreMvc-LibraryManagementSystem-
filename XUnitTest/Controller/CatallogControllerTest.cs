using FluentAssertions;
using LibraryData;
using LibraryData.Model;
using LibraryManagement.Controllers;
using LibraryManagement.ViewModels.Catalog;
using LibraryManagement.ViewModels.CheckOutModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using static LibraryManagement.ViewModels.Catalog.AssetDetailViewModel;

namespace LibraryManagement.Test.Controller
{
   public class CatallogControllerTest
    {

        private static IEnumerable<LibraryAsset> GetAllAssets()
        {
            List<LibraryAsset> allAssets = new List<LibraryAsset>
           {
                new Book
                {
                     Title ="Orlando",
                     Author = "Virginia woolf",
                       Year = 1928,
                    Cost = 23.0M,
                    ImageUrl = "foo",
                    Status = new Status
                    {
                        Name = "Checked In",
                        Id = 1
                    }
                },

                new Video
                {
                     Title = "Happy People",
                    Director = "Werner Herzog",
                    ImageUrl = "images/sample.jpg",
                    Status = new Status
                    {
                        Name = "Lost",
                        Id = 3
                    }
                } 
           };

            return allAssets;
        }


        private static Book GetAsset()
        {
            var book = new Book
            {
                Id = 23,
                Title = "Cameroon",
                Author = "Virginia Woolf",
                Status = new Status
                {
                    Id = 1,
                    Name = "Checked In"
                }

            };
            return book;
        }

        private static AssetHoldModel GetCurrentHold()
        {
            return new AssetHoldModel
            {
                PatronName = "Foo",
                HoldPlaced = "Bar"
            };
        }


        [Fact]
        public void Should_Call_GetById_In_ILibraryAsset_When_Checkout_Called()
        {
            //Arrange

            var mockLibraryAsset = new Mock<ILibraryAsset>();
            var mockCheckOut = new Mock<ICheckOut>();
            mockLibraryAsset.Setup(x => x.GetById(23)).Returns(GetAsset());
            var catalog = new CatalogController(mockLibraryAsset.Object, mockCheckOut.Object);

            //Act
            catalog.Checkout(23);

            //Assert
            mockLibraryAsset.Verify(x => x.GetById(23), Times.Once());

        }

        [Fact]
        public void Should_Return_CheckOutViewModel_When_CheckOut_Called()
        {
            //Arrange
            var mockLibraryAsset = new Mock<ILibraryAsset>();
            var mockCheckOut = new Mock<ICheckOut>();
            mockLibraryAsset.Setup(x => x.GetById(23)).Returns(GetAsset());
            var catalog = new CatalogController(mockLibraryAsset.Object, mockCheckOut.Object);

            //Act
            var result = catalog.Checkout(23);

            //Assert
            var viewresult = result.Should().BeOfType<ViewResult>();
            var viewmodel = viewresult.Subject.ViewData.Model.Should().BeAssignableTo<CheckoutViewModel>();
            viewmodel.Subject.AssetId.Should().Be(23);
            viewmodel.Subject.Title.Should().Be("Cameroon");

        }


        [Fact]
        public void Should_Call_CheckoutItem_In_IChecOut_When_CheckOut_Called()
        {
            var mockLibraryAsset = new Mock<ILibraryAsset>();
            var mockCheckOut = new Mock<ICheckOut>();
            mockLibraryAsset.Setup(x => x.GetById(23)).Returns(GetAsset());
            var catalog = new CatalogController(mockLibraryAsset.Object, mockCheckOut.Object);

            catalog.CheckOut(23, 1);

            mockCheckOut.Verify(x => x.CheckoutItem(23, 1), Times.Once());

        }

        [Fact]
        public void Should_Redirect_To_DetailView_When_CheckOut_Called()
        {
            var mockLibraryAsset = new Mock<ILibraryAsset>();
            var mockCheckOut = new Mock<ICheckOut>();
            mockLibraryAsset.Setup(x => x.GetById(23)).Returns(GetAsset());
            var catalog = new CatalogController(mockLibraryAsset.Object, mockCheckOut.Object);

           var result = catalog.CheckOut(23, 1);

            var redirectresult = result.Should().BeOfType<RedirectToActionResult>();
            redirectresult.Subject.ActionName.Should().Be("Detail");

        }


        [Fact]
        public void Should_Call_CheckInItem_In_ICheckOut_When_CheckIn_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.CheckIn(23);

            mockCheckoutService.Verify(x => x.CheckInItem(23), Times.Once());
        }

        [Fact]
        public void Should_Redirect_To_DetailView_When_CheckIn_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.CheckIn(23);

            var viewresult = result.Should().BeOfType<RedirectToActionResult>();
            viewresult.Subject.ActionName.Should().Be("Detail");
        }

        [Fact]
        public void Should_Call_GetById_In_ILibraryAsset_When_Hold_Called()
        {
            //Arrange

            var mockLibraryAsset = new Mock<ILibraryAsset>();
            var mockCheckOut = new Mock<ICheckOut>();
            mockLibraryAsset.Setup(x => x.GetById(23)).Returns(GetAsset());
            var catalog = new CatalogController(mockLibraryAsset.Object, mockCheckOut.Object);

            //Act
            catalog.Hold(23);

            //Assert
            mockLibraryAsset.Verify(x => x.GetById(23), Times.Once());

        }

        [Fact]
        public void Should_Return_CheckOutViewModel_When_Hold_Called()
        {
            //Arrange
            var mockLibraryAsset = new Mock<ILibraryAsset>();
            var mockCheckOut = new Mock<ICheckOut>();
            mockLibraryAsset.Setup(x => x.GetById(23)).Returns(GetAsset());
            var catalog = new CatalogController(mockLibraryAsset.Object, mockCheckOut.Object);

            //Act
            var result = catalog.Hold(23);

            //Assert
            var viewresult = result.Should().BeOfType<ViewResult>();
            var viewmodel = viewresult.Subject.ViewData.Model.Should().BeAssignableTo<CheckoutViewModel>();
          
        }

        [Fact]
        public void Should_Call_PlaceHold_In_ICheckOut_When_Hold_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Hold(23, 1);

            mockCheckoutService.Verify(x => x.PlaceHold(23, 1), Times.Once());
        }

        [Fact]
        public void Should_Redirect_To_Detail_When_Hold_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Hold(23, 1);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");
        }

        [Fact]
        public void Call_MarkFound_In_IheckOut_When_MarkFound_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.MarkFound(23);

            mockCheckoutService.Verify(s => s.MarkFound(23), Times.Once());
        }


        [Fact]
        public void Should_Redirect_To_Detail_When_MarkFound_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.MarkFound(23);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");

        }

        [Fact]
        public void Call_MarkLost_In_IheckOut_When_MarkLost_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            controller.MarkLost(23);

            mockCheckoutService.Verify(s => s.MarkLost(23), Times.Once());

        }

        [Fact]
        public void Should_Redirect_To_Detail_When_MarkLost_Called()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());
            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.MarkLost(23);
            var redirectResult = result.Should().BeOfType<RedirectToActionResult>();

            redirectResult.Subject.ActionName.Should().Be("Detail");

        }

        [Fact]
        public void Should_Return_AllLibraryAsset()
        {
            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();

            mockLibraryAssetService.Setup(r => r.GetAll()).Returns(GetAllAssets());

            var controller = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = controller.Index();
            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<AssetIndexViewModel>();
            var viewmodel = viewResult.Subject.Model.Should().BeAssignableTo<AssetIndexViewModel>();
            viewmodel.Subject.Assets.Count().Should().Be(2);
            mockLibraryAssetService.Verify(x => x.GetAll(), Times.Once());
        }


        [Fact]
        public void Should_Return_Detail_LibraryAsset()
        {
            //Arrange

            var mockLibraryAssetService = new Mock<ILibraryAsset>();
            var mockCheckoutService = new Mock<ICheckOut>();
            mockLibraryAssetService.Setup(r => r.GetById(23)).Returns(GetAsset());

            mockCheckoutService.Setup(r => r.GetCurrentHoldPlaced(23)).Returns(GetCurrentHold().HoldPlaced);
            mockCheckoutService.Setup(r => r.GetCurrentHoldPatron(23)).Returns(GetCurrentHold().PatronName);

            mockCheckoutService.Setup(r => r.GetCheckoutHistory(23)).Returns(new List<CheckoutHistory>
            {
                new CheckoutHistory()
            });

            mockLibraryAssetService.Setup(r => r.GetType(23)).Returns("Book");
            mockLibraryAssetService.Setup(r => r.GetCurrentLocation(23)).Returns(new LibraryBranch
            {
                Name = "Hawkins Library"
            });
            mockLibraryAssetService.Setup(r => r.GetAuthorOrDirector(23)).Returns("Virginia Woolf");
            mockLibraryAssetService.Setup(r => r.GetDeweyIndex(23)).Returns("ELEVEN");
            mockCheckoutService.Setup(r => r.GetCheckoutHistory(23)).Returns(new List<CheckoutHistory>
            {
                new CheckoutHistory()
            });
            mockCheckoutService.Setup(r => r.GetLatestCheckout(23)).Returns(new Checkouts());
            mockCheckoutService.Setup(r => r.GetCurrentPatron(23)).Returns("NANCY");
            var sut = new CatalogController(mockLibraryAssetService.Object, mockCheckoutService.Object);

            var result = sut.Detail(23);

            var viewResult = result.Should().BeOfType<ViewResult>();
            var viewModel = viewResult.Subject.ViewData.Model.Should().BeAssignableTo<AssetDetailViewModel>();
            viewModel.Subject.Title.Should().Be("Cameroon");
            viewModel.Subject.Type.Should().Be("Book");
            viewModel.Subject.AuthorOrDirector.Should().Be("Virginia Woolf");
            viewModel.Subject.CurrentLocation.Should().Be("Hawkins Library");
            viewModel.Subject.DeweyCallNumber.Should().Be("ELEVEN");
            viewModel.Subject.PatronName.Should().Be("NANCY");

        }

    }
}
