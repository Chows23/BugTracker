using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class ProjectUserService
    {
        private ProjectUserRepo repo;
        public ProjectUserService()
        {
            repo = new ProjectUserRepo();
        }
        public ProjectUserService(ProjectUserRepo repo)
        {
            this.repo = repo;
        }


        public void Create(ProjectUser projectUser)
        {
            repo.Add(projectUser);
        }

        public ProjectUser ProjectUser(string userId, int projectId)
        {
            var projectUser = new ProjectUser
            {
                UserId = userId,
                ProjectId = projectId
            };

            return projectUser;
        }

        public ProjectUser GetProjectUser(int id)
        {
            return repo.GetEntity(id);
        }

        public IEnumerable<ProjectUser> GetAllProjectUsers()
        {
            return repo.GetCollection(null);
        }

        public bool CheckIfUserOnProject(int projectId, string userId)
        {
            var existingProjectUser = GetAllProjectUsers().FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == userId);

            if (existingProjectUser != null)
                return true;
            return false;
        }

        public ProjectUser GetExistingProjectUser(int projectId, string userId)
        {
            return GetAllProjectUsers().FirstOrDefault(pu => pu.ProjectId == projectId && pu.UserId == userId);
        }

        public void RemoveProjectUser(ProjectUser projectUser)
        {
            repo.Delete(projectUser.Id);
        }

        public List<ProjectUser> GetUsersProjects(string userId)
        {
            return repo.GetCollection(pu => pu.UserId == userId).ToList();
        }
    }
}