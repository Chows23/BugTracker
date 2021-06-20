using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_TicketPriorityService
    {
        Mock<TicketPriorityRepo> mockedRepo;
        TicketPriorityService ticketPriorityService;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketPriorityRepo>();

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketPriority>()));

            ticketPriorityService = new TicketPriorityService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_TicketPriority_Will_Call_Add_TicketPriority_On_Repo()
        {
            TicketPriority ticketPriority = new TicketPriority { Id = 1, Name="Low" };
            ticketPriorityService.Create(ticketPriority);
            mockedRepo.Verify(r => r.Add(ticketPriority));
        }
    }
}
