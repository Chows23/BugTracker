using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.BL;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ProjectService projectService = new ProjectService();
        private UserService userService = new UserService();
        public ActionResult Index()
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = (ApplicationUser)User.Identity;
            else
                return new HttpUnauthorizedResult();
            
            return View(user.ProjectUsers.Select(p => p.Project));
        }
    }
}