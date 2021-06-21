using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using PagedList;

namespace Bug_Tracker.BL
{
    public class ProjectService
    {
        private ProjectRepo repo;
        public ProjectService()
        {
            repo = new ProjectRepo();
        }
        public ProjectService(ProjectRepo repo)
        {
            this.repo = repo;
        }


        public void Create(Project project)
        {
            repo.Add(project);
        }

        public Project GetProject(int id)
        {
            return repo.GetEntity(id);
        }

        public IEnumerable<Project> AllProjects()
        {
            return repo.GetCollection();
        }

        public void Update(Project project)
        {
            repo.Update(project);
        }

        public List<Project> GetNProjects(int n, ApplicationUser user)
        {
            if (user == null)
                return repo.GetCollection().Take(n).ToList();
            else
                return user.ProjectUsers.Select(pu => pu.Project).Take(n).ToList();
        }

        public List<Ticket> GetUserTicketsOnProject(string userId, List<Ticket> tickets)
        {
            if (UserService.UserInRole(userId, "submitter"))
                return tickets.Where(t => t.OwnerUserId == userId).ToList();
            else if (UserService.UserInRole(userId, "developer"))
                return tickets.Where(t => t.AssignedToUserId == userId).ToList();
            else
                return tickets;
        }

        public ProjectDetailsViewModel ProjectDetailsViewModel(int id, string name, List<ProjectUser> projectUsers, PagedList.IPagedList<Bug_Tracker.Models.Ticket> tickets)
        {
            var viewModel = new ProjectDetailsViewModel
            {
                Id = id,
                Name = name,
                ProjectUsers = projectUsers,
                Tickets = tickets
            };
            return viewModel;
        }
    }
}