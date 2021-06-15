using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class DashboardViewModels
    {
        public DashboardViewModels()
        {
            ProjectUsers = new List<ProjectUser>();
            Tickets = new List<Ticket>();
        }

        public List<ProjectUser> ProjectUsers;
        public List<Ticket> Tickets;
    }
}