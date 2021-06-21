using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_TicketNotificationService
    {
        Mock<TicketNotificationRepo> mockedRepo;
        TicketNotificationService ticketNotificationService;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketNotificationRepo>();

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketNotification>()));

            ticketNotificationService = new TicketNotificationService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_TicketNotification_Will_Call_Add_TicketNotification_On_Repo()
        {
            TicketNotification ticketNotification = new TicketNotification { Id = 1, TicketId = 100, UserId = "UserId_1" };
            ticketNotificationService.Add(ticketNotification);
            mockedRepo.Verify(r => r.Add(ticketNotification));
        }
    }
}
