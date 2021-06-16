using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;

namespace Bug_Tracker.BL
{
    public class ProjectService
    {
        private ProjectRepo repo = new ProjectRepo();

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
            return repo.GetAll();
        }

        public void Update(Project project)
        {
            repo.Update(project);
        }

        public List<Project> GetNLatestUpdated(int n, ApplicationUser user)
        {
            if (user == null)
                return repo.GetCollection().Take(n).ToList();
            else
            {
                if (user.ProjectUsers.Any(pu => pu.Project.Tickets.Count == 0))
                    return user.ProjectUsers.Select(pu => pu.Project).ToList();

                return user.ProjectUsers.Select
                    (pu => pu.Project).OrderByDescending
                    (p => p.Tickets.Max(t => t.Updated)).Take(n).ToList();
            }
        }

        public List<Ticket> GetUserTicketsOnProject(ApplicationUser user, List<Ticket> tickets)
        {
            if (UserService.UserInRole(user.Id, "submitter"))
                return tickets.Where(t => t.OwnerUserId == user.Id).ToList();
            else if (UserService.UserInRole(user.Id, "developer"))
                return tickets.Where(t => t.AssignedToUserId == user.Id).ToList();
            else
                return tickets;
        }
    }
}