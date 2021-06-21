using Bug_Tracker.BL;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BugTrackerTests
{
    [TestClass]
    public class UnitTest_ProjectService
    {
        Mock<ProjectRepo> mockedRepo;
        ProjectService projectService;

        ApplicationUser user1;
        ApplicationUser user2;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<ProjectRepo>();

            Ticket ticket1 = new Ticket { Id = 1, Title = "Test Ticket 1", Description = "This is a test bug ticket 1.", ProjectId = 1, TicketType = null, TicketPriority = null, TicketStatus = null, OwnerUserId = "1", AssignedToUserId = "2", Created = DateTime.Now.AddDays(-10) };
            Ticket ticket2 = new Ticket { Id = 2, Title = "Test Ticket 2", Description = "This is a test bug ticket 2.", ProjectId = 1, TicketType = null, TicketPriority = null, TicketStatus = null, OwnerUserId = "1", AssignedToUserId = "2", Created = DateTime.Now.AddDays(-9) };
            Project project1 = new Project { Id = 1, Name = "New exiting Project 1" };
            project1.Tickets.Add(ticket1);
            project1.Tickets.Add(ticket2);

            Ticket ticket3 = new Ticket { Id = 3, Title = "Test Ticket 3", Description = "This is a test bug ticket 3.", ProjectId = 2, TicketType = null, TicketPriority = null, TicketStatus = null, OwnerUserId = "1", AssignedToUserId = "2", Created = DateTime.Now };
            Project project2 = new Project { Id = 2, Name = "New exiting Project 2" };
            project2.Tickets.Add(ticket3);

            Ticket ticket4 = new Ticket { Id = 4, Title = "Test Ticket 4", Description = "This is a test bug ticket 4.", ProjectId = 5, TicketType = null, TicketPriority = null, TicketStatus = null, OwnerUserId = "1", AssignedToUserId = "2", Created = DateTime.Now.AddDays(-1) };
            Project project5 = new Project { Id = 5, Name = "New exiting Project 5" };
            project5.Tickets.Add(ticket4);

            Project project6 = new Project { Id = 6, Name = "New exiting Project 6" };
  
            Project project7 = new Project { Id = 7, Name = "New exiting Project 7" };

            Ticket ticket5 = new Ticket { Id = 5, Title = "Test Ticket 5", Description = "This is a test bug ticket 5.", ProjectId = 8, TicketType = null, TicketPriority = null, TicketStatus = null, OwnerUserId = "1", AssignedToUserId = "2", Created = DateTime.Now };
            Project project8 = new Project { Id = 8, Name = "New exiting Project 8" };
            project8.Tickets.Add(ticket5);

            IEnumerable<Project> projects = new List<Project> { project1, project2, project5, project6, project7, project8 };

            user1 = new ApplicationUser();
            ProjectUser projectUser1 = new ProjectUser { Id = 1, Project = project1, User = user1 };
            ProjectUser projectUser2 = new ProjectUser { Id = 2, Project = project2, User = user1 };
            ProjectUser projectUser5 = new ProjectUser { Id = 5, Project = project5, User = user1 };
            projectUser1.Project = project1;
            projectUser2.Project = project2;
            projectUser5.Project = project5;
            user1.ProjectUsers.Add(projectUser1);
            user1.ProjectUsers.Add(projectUser2);
            user1.ProjectUsers.Add(projectUser5);
            projectUser5.User = user1;
            projectUser5.Project = project5;

            user2 = new ApplicationUser();
            ProjectUser projectUser6 = new ProjectUser { Id = 6, Project = project6, User = user2 };
            ProjectUser projectUser7 = new ProjectUser { Id = 7, Project = project7, User = user2 };
            ProjectUser projectUser8 = new ProjectUser { Id = 8, Project = project8, User = user2 };
            user2.ProjectUsers.Add(projectUser6);
            user2.ProjectUsers.Add(projectUser7);
            user2.ProjectUsers.Add(projectUser8);
            projectUser8.User = user2;
            projectUser8.Project = project8;

            mockedRepo.Setup(r => r.Add(It.IsAny<Project>()));
            mockedRepo.Setup(r => r.GetEntity(1)).Returns(project1);
            mockedRepo.Setup(r => r.GetEntity(2)).Returns(project2);
            mockedRepo.Setup(r => r.GetEntity(5)).Returns(project5);
            mockedRepo.Setup(r => r.GetAll()).Returns(projects);
            mockedRepo.Setup(r => r.GetCollection()).Returns(projects);

            projectService = new ProjectService(mockedRepo.Object);
        }


        [TestMethod]
        public void Create_Project_Should_Call_Add_Project_On_Repo()
        {
            Project project = new Project { Id = 1, Name = "New exiting Project" };
            projectService.Create(project);
            mockedRepo.Verify(r => r.Add(It.IsAny<Project>()));
        }


        [TestMethod]
        public void GetProject_By_Id_Should_Fetch_Project_By_Id_From_Repo()
        {
            Project project1 = projectService.GetProject(1);
            Project project2 = projectService.GetProject(2);
            Project project5 = projectService.GetProject(5);

            mockedRepo.Verify(r => r.GetEntity(It.IsAny<int>()));
            Assert.AreEqual(project1.Id, 1);
            Assert.AreEqual(project2.Id, 2);
            Assert.AreEqual(project5.Id, 5);
        }


        [TestMethod]
        public void AllProjects_Should_Fetch_Projects_By_Calling_GetAll_From_Repo()
        {
            List<Project> projects = projectService.AllProjects().ToList();

            mockedRepo.Verify(r => r.GetAll());
            Assert.IsTrue(projects.Exists(p => p.Id == 1));
            Assert.IsTrue(projects.Exists(p => p.Id == 2));
            Assert.IsTrue(projects.Exists(p => p.Id == 5));
            Assert.IsTrue(projects.Exists(p => p.Id == 6));
            Assert.IsTrue(projects.Exists(p => p.Id == 7));
            Assert.IsTrue(projects.Exists(p => p.Id == 8));
            Assert.AreEqual(projects.Count, 6);
        }


        [TestMethod]
        public void Update_Project_Should_Call_Update_Project_On_Repo()
        {
            Project project = new Project { Id = 1, Name = "New exiting Project" };
            projectService.Update(project);
            mockedRepo.Verify(r => r.Update(It.IsAny<Project>()));
        }


        [TestMethod]
        public void GetNProjects_Should_Call_GetCollection_On_Repo_If_User_Null()
        {
            List<Project> projects = projectService.GetNProjects(2, null);
            mockedRepo.Verify(r => r.GetCollection());
        }


        [TestMethod]
        public void GetNProjects_With_User_Null_Should_Return_First_N_Projects()
        {
            List<Project> projects = projectService.GetNProjects(3, null);

            mockedRepo.Verify(r => r.GetCollection());
            Assert.AreEqual(projects.Count, 3);
            Assert.IsTrue(projects.Exists(p => p.Id == 1));
            Assert.IsTrue(projects.Exists(p => p.Id == 2));
            Assert.IsTrue(projects.Exists(p => p.Id == 5));
        }


        [TestMethod]
        public void GetNProjects_Should_Return_First_N_Projects_Of_User()
        {
            List<Project> projects = projectService.GetNProjects(3, user2);

            Assert.AreEqual(projects.Count, 3);
            Assert.IsTrue(projects.Exists(p => p.Id == 6));
            Assert.IsTrue(projects.Exists(p => p.Id == 7));
            Assert.IsTrue(projects.Exists(p => p.Id == 8));
        }
    }
}
