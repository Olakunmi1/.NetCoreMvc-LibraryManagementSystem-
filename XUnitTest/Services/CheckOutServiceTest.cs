using LibraryData;
using LibraryData.Model;
using LibraryServices;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LibraryManagement.Test.Services
{
    public class CheckOutServiceTest
    {

        [Fact]
        public void Should_Add_New_CheckOut_and_Calls_SaveChanges()
        {
            var options = new DbContextOptionsBuilder<LibraryDBContext>().UseSqlServer("Add_writes_to_database")
                 .Options;

            using (var context = new LibraryDBContext(options))
            {
                var service = new CheckOutServices(context);

                service.Add(new Checkouts
                {
                    Id = -247
                });

                Assert.Equal(247, context.Checkouts.Single().Id);

            }


        }

    }
}
