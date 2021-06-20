using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_TicketHistoryService
    {

        Mock<TicketHistoryRepo> mockedRepo;
        TicketHistoryService ticketHistoryService;
        ApplicationUser user;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketHistoryRepo>();

            TicketHistory ticketHistory1 = new TicketHistory { Id = 1, UserId = "UserId_1", TicketId = 1, Property = "Some property", OldValue = "one", NewValue = "two", Changed = DateTime.Now.AddDays(-10) };
            List<TicketHistory> ticketHistories = new List<TicketHistory> { ticketHistory1 };

            user = new ApplicationUser();
            user.TicketHistories.Add(ticketHistory1);

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketHistory>()));

            ticketHistoryService = new TicketHistoryService(mockedRepo.Object);
        }


        [TestMethod]
        public void Create_TicketHistory_Should_Call_Add_TicketHistory_On_Repo()
        {
            TicketHistory ticketHistory = new TicketHistory { Id = 1, UserId = "UserId_1", TicketId = 1, Property = "Some property", OldValue = "one", NewValue = "two", Changed = DateTime.Now.AddDays(-10) };
            ticketHistoryService.Create(ticketHistory);
            mockedRepo.Verify(r => r.Add(ticketHistory));
        }


        [TestMethod]
        public void CompareTickets_Should_Create_TicketHistory_For_Each_Difference_And_Call_Add_TicketHistory_On_Repo()
        {
            Ticket ticket_Old = new Ticket { Id = 1, Title = "Test Ticket 1", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-10), Updated = DateTime.Now.AddDays(-1) };
            Ticket ticket_New = new Ticket { Id = 1, Title = "Changed Title", Description = "New Description", Created = DateTime.Now.AddDays(-10), Updated = DateTime.Now };

            ticketHistoryService.CompareTickets(ticket_Old, ticket_New);
            mockedRepo.Verify(r => r.Add(It.Is<TicketHistory>(th => th.Property == "Title")));
            mockedRepo.Verify(r => r.Add(It.Is<TicketHistory>(th => th.Property == "Description")));
        }
    }
}
