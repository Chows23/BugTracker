using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_TicketAttachmentService
    {
        Mock<TicketAttachmentRepo> mockedRepo;
        TicketAttachmentService ticketAttachmentService;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketAttachmentRepo>();

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketAttachment>()));

            ticketAttachmentService = new TicketAttachmentService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_TicketAttachment_Will_Call_Add_TicketAttachment_On_Repo()
        {
            TicketAttachment ticketAttachment = new TicketAttachment { Id = 1, Created = DateTime.Now, FilePath = "/", Description = "Cat picture", TicketId = 1, UserId = "UserId_1" };
            ticketAttachmentService.Create(ticketAttachment);
            mockedRepo.Verify(r => r.Add(ticketAttachment));
        }

        [TestMethod]
        public void TicketAttachment_Will_Return_New_TicketAttachment_Object()
        {
            TicketAttachment ticketAttachment = ticketAttachmentService.TicketAttachment(1, "/", "Cat picture", "UserId_1", "/");
            Assert.IsNotNull(ticketAttachment);
            Assert.IsTrue(ticketAttachment.TicketId == 1);
            Assert.IsTrue(ticketAttachment.UserId == "UserId_1");
        }

    }
}
