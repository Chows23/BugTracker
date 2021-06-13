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
        private ProjectUserRepo repo { get; set; }
        public void Create(ProjectUser projectUser)
        {
            repo.Add(projectUser);
        }
    }
}