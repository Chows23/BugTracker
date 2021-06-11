using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Project
    {
        public Project()
        {
            this.Tickets = new HashSet<Ticket>();
            this.ProjectUsers = new HashSet<ProjectUser>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}