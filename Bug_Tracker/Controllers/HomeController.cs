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
            {
                if (User.IsInRole("admin"))
                {
                    var recentTickets = db.Tickets.OrderByDescending(t => t.Updated).Take(5).ToList();
                    return RedirectToAction("Dashboard", recentTickets);
                }
                else if (User.IsInRole("manager"))
                {
                    var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                    var recentProjects = User.Identity;
                    return RedirectToAction("Dashboard", recentProjects);
                }
                else if (User.IsInRole("developer"))
                {
                    return View();
                }
                else if (User.IsInRole("submitter"))
                {
                    return View();
                }
                else
                    return View();
            }
            else
                return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}