using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            else
                return View();
        }

        public ActionResult Dashboard()
        {
            if (User.IsInRole("admin"))
            {
                var recentTickets = db.Tickets.OrderByDescending(t => t.Updated).Take(5).ToList();

                return View(recentTickets);
            }
            else if (User.IsInRole("manager"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                //var recentProjects = user.ProjectUsers.OrderByDescending
                //    (p => p.Project.Tickets.OrderByDescending
                //    (t => t.Updated)).Take(5).ToList();
                //FIX ^^^^^^^^^^^^^^^^^^^^^

                var recentProjects = user.ProjectUsers.Take(5).ToList();

                return View(recentProjects);
            }
            else if (User.IsInRole("developer"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return View();
            }
            else if (User.IsInRole("submitter"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                return View();
            }
            else
                return View();
        }
    }
}