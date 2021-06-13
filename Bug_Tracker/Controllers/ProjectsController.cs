using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.BL;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ProjectService projectService = new ProjectService();
        private UserService userService = new UserService();
        public static ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        public ActionResult Index()
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = userManager.FindById(User.Identity.GetUserId());
            else
                return new HttpUnauthorizedResult();
            
            return View(user.ProjectUsers.Select(p => p.Project));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Project project)
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = userManager.FindById(User.Identity.GetUserId());
            else
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                projectService.Create(project);
                return RedirectToAction("Edit", "Projects", new { id = project.Id });
            }

            return View(project);
        }
    }
}