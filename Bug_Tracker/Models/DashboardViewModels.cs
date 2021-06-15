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
            Projects = new List<Project>();
            Tickets = new List<Ticket>();
        }

        public List<Project> Projects;
        public List<Ticket> Tickets;
    }
}