using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_TicketCommentService
    {

        Mock<TicketCommentRepo> mockedRepo;
        Mock<Ticket> mockedTicket;
        TicketCommentService ticketCommentService;


        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketCommentRepo>();
            mockedTicket = new Mock<Ticket>();

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketComment>()));
            mockedTicket.Setup(t => t.TicketComments.Add(It.IsAny<TicketComment>()));

            ticketCommentService = new TicketCommentService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_TicketComment_Will_Call_Add_TicketComment_On_Ticket()
        {
            TicketComment ticketComment = new TicketComment { Id = 1, Comment = "New nice comment", TicketId = 1, UserId = "1" };
            ticketCommentService.Create(ticketComment, mockedTicket.Object);
            mockedTicket.Verify(t => t.TicketComments.Add(It.IsAny<TicketComment>()));
        }

        [TestMethod]
        public void Create_TicketComment_Will_Add_TicketComment_To_Ticket()
        {
            TicketComment ticketComment = new TicketComment { Id = 1, Comment = "New nice comment", TicketId = 1, UserId = "1" };
            Ticket ticket = new Ticket
            {
                Title = "Test Ticket",
                Description = "This is a test bug ticket.",
                Project = null,
                TicketType = null,
                TicketPriority = null,
                TicketStatus = null,
                OwnerUserId = "1",
                AssignedToUserId = "2",
                Created = DateTime.Now,
            };
            ticketCommentService.Create(ticketComment, ticket);

            Assert.AreEqual(ticket.TicketComments.Count, 1);
            Assert.IsTrue(ticket.TicketComments.Contains(ticketComment));
        }

        [TestMethod]
        public void Create_TicketComment_Will_Call_Add_TicketComment_On_Repo()
        {
            TicketComment ticketComment = new TicketComment { Id = 1, Comment = "New nice comment", TicketId = 1, UserId = "1" };
            ticketCommentService.Create(ticketComment, mockedTicket.Object);
            mockedRepo.Verify(r => r.Add(It.IsAny<TicketComment>()));
        }
    }
}
