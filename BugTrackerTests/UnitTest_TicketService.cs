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
    public class UnitTest_TicketService
    {
        Mock<TicketRepo> mockedRepo;
        TicketService ticketService;
        ApplicationUser user;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketRepo>();

            Ticket ticket1 = new Ticket { Id = 1, Title = "Test Ticket 1", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-10), Updated = DateTime.Now.AddDays(-1) };
            Ticket ticket2 = new Ticket { Id = 2, Title = "Test Ticket 2", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-9), Updated = DateTime.Now.AddDays(-10) };
            Ticket ticket3 = new Ticket { Id = 3, Title = "Test Ticket 3", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-8), Updated = DateTime.Now };
            List<Ticket> tickets = new List<Ticket> { ticket1, ticket2, ticket3 };
            List<Ticket> ticketsLatest = new List<Ticket> { ticket3, ticket2, ticket1 };

            user = new ApplicationUser();
            user.Tickets.Add(ticket1);
            user.Tickets.Add(ticket2);
            user.Tickets.Add(ticket3);

            mockedRepo.Setup(r => r.Add(It.IsAny<Ticket>()));
            mockedRepo.Setup(r => r.GetEntity(1)).Returns(ticket1);
            mockedRepo.Setup(r => r.GetEntity(2)).Returns(ticket2);
            mockedRepo.Setup(r => r.GetEntity(3)).Returns(ticket3);
            mockedRepo.Setup(r => r.GetCollection()).Returns((IEnumerable<Ticket>)tickets);
            mockedRepo.Setup(r => r.GetCollection(It.IsAny<Func<Ticket, DateTime>>())).Returns((IEnumerable<Ticket>)ticketsLatest);
            mockedRepo.Setup(r => r.GetCollection(It.IsAny<Func<Ticket, bool>>())).Returns((IEnumerable<Ticket>)ticketsLatest);

            ticketService = new TicketService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_Ticket_Should_Call_Add_Ticket_On_Repo()
        {
            Ticket ticket = new Ticket { Id = 1, Title = "Test Ticket 1", Description = "This is a test bug ticket.", Created = DateTime.Now };
            ticketService.Create(ticket);
            mockedRepo.Verify(r => r.Add(ticket));
        }

        [TestMethod]
        public void GetTicket_Should_Fetch_Ticket_On_Repo_By_Id()
        {
            Ticket ticket1 = ticketService.GetTicket(1);
            Ticket ticket2 = ticketService.GetTicket(2);
            Ticket ticket3 = ticketService.GetTicket(3);
            Assert.IsNotNull(ticket1);
            Assert.IsNotNull(ticket2);
            Assert.IsNotNull(ticket3);
            Assert.IsTrue(ticket1.Title == "Test Ticket 1");
            Assert.IsTrue(ticket2.Title == "Test Ticket 2");
            Assert.IsTrue(ticket3.Title == "Test Ticket 3");
        }

        [TestMethod]
        public void GetNLatestUpdated_With_User_Null_Should_Return_N_Latest_Updated_Tickets()
        {
            List<Ticket> tickets = ticketService.GetNLatestUpdated(1, null);
            Assert.IsTrue(tickets.Count == 1);
            Assert.IsTrue(tickets.Exists(t => t.Id == 3));

            tickets = ticketService.GetNLatestUpdated(2, null);
            Assert.IsTrue(tickets.Count == 2);
            Assert.IsTrue(tickets.Exists(t => t.Id == 2));
            Assert.IsTrue(tickets.Exists(t => t.Id == 3));
        }

        [TestMethod]
        public void ChangeDeveloper_Should_Call_Update_On_Repo()
        {
            Ticket ticket = new Ticket { Id = 1, Title = "Test Ticket 1" };
            ticketService.ChangeDeveloper(ticket, user);
            mockedRepo.Verify(r => r.Update(ticket, user));
        }


        [TestMethod]
        public void RemoveTicketUser_Should_Call_Update_On_Repo()
        {
            Ticket ticket = new Ticket { Id = 1, Title = "Test Ticket 1" };
            ticketService.RemoveTicketUser(ticket);
            mockedRepo.Verify(r => r.Update(ticket, null));
        }


        [TestMethod]
        public void GetUserTickets_With_User_Null_Should_Call_GetCollections()
        {
            List<Ticket> ticket = ticketService.GetUserTickets(null);
            mockedRepo.Verify(r => r.GetCollection(It.IsAny<Func<Ticket, bool>>()));
        }

        [TestMethod]
        public void GetOwnerTickets_Should_Call_GetCollection()
        {
            List<Ticket> ticket = ticketService.GetUserTickets(null);
            mockedRepo.Verify(r => r.GetCollection(It.IsAny<Func<Ticket, bool>>()));
        }
    }
}
