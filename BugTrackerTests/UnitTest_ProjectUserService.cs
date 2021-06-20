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
    public class UnitTest_ProjectUserService
    {
        Mock<ProjectUserRepo> mockedRepo;
        ProjectUserService projectUserService;

        [TestInitialize]
        public void SetUp()
        {
            mockedRepo = new Mock<ProjectUserRepo>();

            ProjectUser projectUser1 = new ProjectUser { Id = 1, ProjectId = 100, UserId = "UserId_1" };
            ProjectUser projectUser2 = new ProjectUser { Id = 2, ProjectId = 200, UserId = "UserId_2" };
            ProjectUser projectUser3 = new ProjectUser { Id = 3, ProjectId = 300, UserId = "UserId_3" };

            IEnumerable<ProjectUser> projectUsers = new List<ProjectUser> { projectUser1, projectUser2, projectUser3 };


            mockedRepo.Setup(r => r.Add(It.IsAny<ProjectUser>()));
            mockedRepo.Setup(r => r.GetEntity(1)).Returns(projectUser1);
            mockedRepo.Setup(r => r.GetCollection(null)).Returns(projectUsers);
            mockedRepo.Setup(r => r.Delete(It.IsAny<int>()));

            projectUserService = new ProjectUserService(mockedRepo.Object);
        }

        [TestMethod]
        public void Create_ProjectUser_Should_Call_Add_ProjectUser_On_Repo()
        {
            ProjectUser projectUser = new ProjectUser { Id = 1, ProjectId = 1, UserId = "1" };
            projectUserService.Create(projectUser);
            mockedRepo.Verify(r => r.Add(It.IsAny<ProjectUser>()));
        }

        [TestMethod]
        public void ProjectUser_Should_Create_And_Return_New_ProjectUser()
        {
            ProjectUser projectUser = projectUserService.ProjectUser("UserId_1", 1);
            Assert.IsTrue(projectUser.UserId == "UserId_1");
            Assert.IsTrue(projectUser.ProjectId == 1);
        }

        [TestMethod]
        public void GetProjectUser_Should_Fetch_ProjectUser_By_Id()
        {
            ProjectUser projectUser = projectUserService.GetProjectUser(1);
            Assert.IsTrue(projectUser.Id == 1);
            Assert.IsTrue(projectUser.UserId == "UserId_1");
            Assert.IsTrue(projectUser.ProjectId == 100);
        }

        [TestMethod]
        public void GetAllProjectUsers_Should_Call_GetCollection_On_Repo()
        {
            IEnumerable<ProjectUser> projectUsers = projectUserService.GetAllProjectUsers();
            List<ProjectUser> projectUsersList = new List<ProjectUser>(projectUsers);

            mockedRepo.Verify(r => r.GetCollection(null));
            Assert.IsTrue(projectUsersList.Count == 3);
            Assert.IsTrue(projectUsersList.Exists(pu => pu.Id == 1));
            Assert.IsTrue(projectUsersList.Exists(pu => pu.Id == 2));
            Assert.IsTrue(projectUsersList.Exists(pu => pu.Id == 3));
        }

        [TestMethod]
        public void CheckIfUserOnProject_Should_Return_True_If_ProjectUser_Exist()
        {
            Assert.IsTrue(projectUserService.CheckIfUserOnProject(100, "UserId_1"));
            Assert.IsTrue(projectUserService.CheckIfUserOnProject(200, "UserId_2"));
            Assert.IsTrue(projectUserService.CheckIfUserOnProject(300, "UserId_3"));
            Assert.IsFalse(projectUserService.CheckIfUserOnProject(400, "UserId_4"));
        }


        [TestMethod]
        public void GetExistingProjectUser_Should_Return_Existing_ProjectUser_By_UserID_And_ProjectId()
        {
            ProjectUser projectUser1 = projectUserService.GetExistingProjectUser(100, "UserId_1");
            ProjectUser projectUser2 = projectUserService.GetExistingProjectUser(200, "UserId_2");
            ProjectUser projectUser3 = projectUserService.GetExistingProjectUser(300, "UserId_3");
            ProjectUser projectUser4 = projectUserService.GetExistingProjectUser(400, "UserId_4");
            ProjectUser projectUser1_2 = projectUserService.GetExistingProjectUser(100, "UserId_1");

            Assert.IsTrue(projectUser1.Id == 1);
            Assert.IsTrue(projectUser2.Id == 2);
            Assert.IsTrue(projectUser3.Id == 3);
            Assert.AreEqual(projectUser1, projectUser1_2);
            Assert.IsNull(projectUser4);
        }

        [TestMethod]
        public void RemoveProjectUser_Should_Call_Delete_By_Id_On_Repo()
        {
            ProjectUser projectUser = new ProjectUser { Id = 1, ProjectId = 1, UserId = "1" };
            projectUserService.RemoveProjectUser(projectUser);
            mockedRepo.Verify(r => r.Delete(1));
        }
    }
}
