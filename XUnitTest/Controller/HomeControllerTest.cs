using FluentAssertions;
using LibraryManagement.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LibraryManagement.Test.Controller
{
   public class HomeControllerTest
    {
        [Fact]
        public void Return_Home_page()
        {
            //Arrange
            var controller = new HomeController();

            //Act
            var result = controller.Index();
            result.Should().BeOfType<ViewResult>();

        }
    }
}
