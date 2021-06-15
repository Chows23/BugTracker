using Bug_Tracker.BL;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
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
        private TicketService ticketService = new TicketService();
        private ProjectService projectService = new ProjectService();

        public ActionResult Index()
        {
            if ((System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                return RedirectToAction("Dashboard");
            else
                return View();
        }

        [Authorize]
        public ActionResult Dashboard()
        {
            if (User.IsInRole("admin"))
            {
                var recentTickets = ticketService.GetNLatestUpdated(3, null);
                var recentProjects = projectService.GetNLatestUpdated(3, null);

                var dashboardViewModel = new DashboardViewModels
                {
                    Tickets = recentTickets,
                    Projects = recentProjects,
                };

                return View(dashboardViewModel);
            }
            else if (User.IsInRole("manager"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var recentProjects = projectService.GetNLatestUpdated(3, user);

                var dashboardViewModel = new DashboardViewModels
                {
                    Projects = recentProjects,
                };

                return View(dashboardViewModel);
            }
            else if (User.IsInRole("developer"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var recentTickets = ticketService.GetNLatestUpdated(3, user);

                var dashboardViewModel = new DashboardViewModels
                {
                    Tickets = recentTickets,
                };

                return View(dashboardViewModel);
            }
            else if (User.IsInRole("submitter"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var recentTickets = ticketService.GetNLatestCreated(3, user);

                var dashboardViewModel = new DashboardViewModels
                {
                    Tickets = recentTickets,
                };

                return View(dashboardViewModel);
            }
            else
                return RedirectToAction("Index");
        }
    }
}