using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_TicketTypeService
    {
        Mock<TicketTypeRepo> mockedRepo;
        TicketTypeService ticketTypeService;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketTypeRepo>();

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketType>()));

            ticketTypeService = new TicketTypeService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_TicketType_Will_Call_Add_TicketType_On_Repo()
        {
            TicketType ticketType = new TicketType { Id = 1, Name="Main" };
            ticketTypeService.Create(ticketType);
            mockedRepo.Verify(r => r.Add(ticketType));
        }
    }
}
