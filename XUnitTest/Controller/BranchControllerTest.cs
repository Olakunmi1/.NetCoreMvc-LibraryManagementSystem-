using FluentAssertions;
using LibraryData;
using LibraryData.Model;
using LibraryManagement.Controllers;
using LibraryManagement.ViewModels.Branch;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LibraryManagement.Test
{
    public class BranchControllerTest
    {

        private static LibraryBranch GetBranchById()
        {
            var librarybranch = new LibraryBranch
            {
                Id = 678,
                Name = "Momodu",
                Address = "3, Acour jucntion"
            };

            return librarybranch;
        }


        private static IEnumerable<LibraryBranch> GetAllBranches()
        {
            List<LibraryBranch> Allbranches = new List<LibraryBranch>
            {
                 new LibraryBranch
                 {
                      Id = 123,
                      Name = "Abiodun",
                      Address = "1, Main close"
                 },

                 new LibraryBranch
                 {
                       Id = 345,
                      Name = "Castle",
                      Address = "1, Main jucntion"

                 },

                 new LibraryBranch
                 {
                       Id = 678,
                      Name = "Momodu",
                      Address = "3, Acour jucntion"

                 }
            };

            return Allbranches;
        }


        [Fact]
        public void Index_Action_method_Should_Return_ListOfBranches()
        {
            // Arrange ---intializing the classes needed, and Setup up Mock

            var mockbranchservice = new Mock<ILibraryBranch>();
            var mockbranch = new Mock<IWebHostEnvironment>();
            mockbranchservice.Setup(x => x.GetAll()).Returns(GetAllBranches());

            var controller = new BranchController(mockbranchservice.Object, mockbranch.Object);

            //Act  --- Calling on the method to be tested 

            var result = controller.Index();

            //Assert  --- i.e we need to start testing d outcome

            var viewresult = result.Should().BeOfType<ViewResult>();
            var viewmodel = viewresult.Subject.ViewData.Model.Should().BeAssignableTo<branchIndexModel>();
            viewmodel.Subject.Branches.Count().Should().Be(3);

        }

        [Fact]
        public void Detail_Action_method_Should_Return_SpecificBranch_WithAn_ID()
        {
            //Arrange
            var mockbranchservice = new Mock<ILibraryBranch>();
            var mockbranch = new Mock<IWebHostEnvironment>();
            mockbranchservice.Setup(x => x.GetById(678)).Returns(GetBranchById());
            var controller = new BranchController(mockbranchservice.Object, mockbranch.Object);

            //Act
            var result = controller.Detail(678);

            //Assert

            //mockbranchservice.Verify(x => x.GetById(678), Times.Once());

            var viewresult = result.Should().BeOfType<ViewResult>();
            var viewmodel = viewresult.Subject.ViewData.Model.Should().BeAssignableTo<branchDetailViewModel>();
            viewmodel.Subject.Id.Should().Be(678);
            viewmodel.Subject.BranchName.Should().Be("Momodu");
            viewmodel.Subject.Address.Should().Be("3, Acour jucntion");
        }

        [Fact]
        public void Should_Return_Type_BranchIndexModel()
        {
            var mockBranchService = new Mock<ILibraryBranch>();
            var mockbranch = new Mock<IWebHostEnvironment>();
            mockBranchService.Setup(r => r.GetAll()).Returns(GetAllBranches());
            var controller = new BranchController(mockBranchService.Object, mockbranch.Object);

            var result = controller.Index();

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<branchIndexModel>();
        }

        [Fact]
        public void Should_Return_Type_BranchDetailModel()
        {
            var mockBranchService = new Mock<ILibraryBranch>();
            var mockbranch = new Mock<IWebHostEnvironment>();
            mockBranchService.Setup(r => r.GetById(678)).Returns(GetBranchById());
            var controller = new BranchController(mockBranchService.Object, mockbranch.Object);

            var result = controller.Detail(678);

            var viewResult = result.Should().BeOfType<ViewResult>();
            viewResult.Subject.Model.Should().BeOfType<branchDetailViewModel>();

        }
    }
}
