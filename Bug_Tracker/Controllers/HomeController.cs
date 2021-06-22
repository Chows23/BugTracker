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
        ApplicationDbContext db = new ApplicationDbContext(); // REMOVE??

        private TicketService ticketService = new TicketService();
        private ProjectService projectService = new ProjectService();
        private TicketStatusService ticketStatusService = new TicketStatusService();
        private UserService userService = new UserService();
        private TicketNotificationService ticketNotificationService = new TicketNotificationService();

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
                var recentTickets = ticketService.GetNLatestUpdated(5, null);
                var projects = projectService.GetNProjects(5, null);

                var dashboardViewModel = new DashboardViewModels
                {
                    Tickets = recentTickets,
                    Projects = projects,
                };

                return View(dashboardViewModel);
            }
            else if (User.IsInRole("manager"))
            {
                var user = UserService.GetUser(User.Identity.Name);
                var projects = projectService.GetNProjects(5, user);

                var dashboardViewModel = new DashboardViewModels
                {
                    Projects = projects,
                };

                return View(dashboardViewModel);
            }
            else if (User.IsInRole("developer"))
            {
                var user = UserService.GetUser(User.Identity.Name);
                var recentTickets = ticketService.GetNLatestUpdated(5, user);

                var dashboardViewModel = new DashboardViewModels
                {
                    Tickets = recentTickets,
                };

                ViewBag.Notifications = ticketNotificationService.GetNotifCount(user.Id);
                return View(dashboardViewModel);
            }
            else if (User.IsInRole("submitter"))
            {
                var user = UserService.GetUser(User.Identity.Name);
                var recentTickets = ticketService.GetNLatestCreated(5, user);

                var dashboardViewModel = new DashboardViewModels
                {
                    Tickets = recentTickets,
                };

                return View(dashboardViewModel);
            }
            else
                return RedirectToAction("Restricted", "Account");
        }

        public JsonResult GetPieChartJSON()
        {
            var user = UserService.GetUser(User.Identity.Name);
            List<DashboardTicketChart> list = new List<DashboardTicketChart>();

            if (User.IsInRole("admin") || User.IsInRole("manager"))
                list = ticketStatusService.GetChartData(null);
            else if (User.IsInRole("developer") || User.IsInRole("submitter"))
                list = ticketStatusService.GetChartData(user);

            return Json(new { JSONList = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPieChartJSON2()
        {
            List<DashboardDevChart> list = new List<DashboardDevChart>();
            list = userService.GetChartData();

            return Json(new { JSONList = list }, JsonRequestBehavior.AllowGet);
        }
    }
}