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

    public class DashboardTicketChart
    {
        public string Status { get; set; }
        public int? StatusCount { get; set; }
    }

    public class DashboardDevChart
    {
        public string Developer { get; set; }
        public int? TicketCount { get; set; }
    }
}