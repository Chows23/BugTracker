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
    public class UnitTest_TicketStatusService
    {
        Mock<TicketStatusRepo> mockedRepo;
        TicketStatusService ticketStatusService;
        ApplicationUser user;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<TicketStatusRepo>();

            Ticket ticket1 = new Ticket { Id = 1, TicketStatusId = 1, Title = "Test Ticket 1", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-10), Updated = DateTime.Now.AddDays(-1) };
            Ticket ticket2 = new Ticket { Id = 2, TicketStatusId = 1, Title = "Test Ticket 2", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-9), Updated = DateTime.Now.AddDays(-10) };
            Ticket ticket3 = new Ticket { Id = 3, TicketStatusId = 2, Title = "Test Ticket 3", Description = "This is a test bug ticket.", Created = DateTime.Now.AddDays(-8), Updated = DateTime.Now };

            user = new ApplicationUser();
            user.Tickets.Add(ticket1);
            user.Tickets.Add(ticket3);

            TicketStatus status1 = new TicketStatus { Id = 1, Name = "Unresolved" };
            TicketStatus status2 = new TicketStatus { Id = 2, Name = "Abandoned" };
            TicketStatus status3 = new TicketStatus { Id = 3, Name = "Resolved" };

            status1.Tickets.Add(ticket1);
            status1.Tickets.Add(ticket2);
            status2.Tickets.Add(ticket3);

            List<TicketStatus> statuses = new List<TicketStatus> { status1, status2, status3 };

            mockedRepo.Setup(r => r.Add(It.IsAny<TicketStatus>()));
            mockedRepo.Setup(r => r.GetCollection()).Returns(statuses);

            ticketStatusService = new TicketStatusService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_TicketStatus_Will_Call_Add_TicketStatus_On_Repo()
        {
            TicketStatus ticketStatus = new TicketStatus { Id = 1, Name = "Some Name" };
            ticketStatusService.Create(ticketStatus);
            mockedRepo.Verify(r => r.Add(ticketStatus));
        }

        [TestMethod]
        public void GetChartData_With_User_Null_Will_Return_Count_Of_TicketStatus()
        {
            List<DashboardTicketChart> chartData = ticketStatusService.GetChartData(null);

            mockedRepo.Verify(r => r.GetCollection());
            Assert.IsTrue(chartData.Count == 3);
            Assert.IsTrue(chartData.Exists(c => c.Status == "Unresolved"));
            Assert.IsTrue(chartData.Exists(c => c.Status == "Abandoned"));
            Assert.IsTrue(chartData.Exists(c => c.Status == "Resolved"));
            Assert.IsTrue(chartData.Find(c => c.Status == "Unresolved").StatusCount == 2);
            Assert.IsTrue(chartData.Find(c => c.Status == "Abandoned").StatusCount == 1);
            Assert.IsTrue(chartData.Find(c => c.Status == "Resolved").StatusCount == 0);
        }
    }
}
