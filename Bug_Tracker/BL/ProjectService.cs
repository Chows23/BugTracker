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

        public List<Project> GetNLatestUpdated(int n, ApplicationUser user)
        {
            if (user == null)
                return repo.GetCollection().Take(n).ToList();
            else
            {
                return user.ProjectUsers.Select
                    (pu => pu.Project).OrderByDescending
                    (p => p.Tickets.Max(t => t.Updated)).Take(n).ToList();
            }
        }
    }
}