using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Bug_Tracker.Models
{
    public class ProjectDetailsViewModel
    {
        public ProjectDetailsViewModel()
        {
            ProjectUsers = new List<ProjectUser>();
            //Tickets = new PagedList.IPagedList<Bug_Tracker.Models.Ticket>
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectUser> ProjectUsers;
        public PagedList.IPagedList<Bug_Tracker.Models.Ticket> Tickets;
    }
}