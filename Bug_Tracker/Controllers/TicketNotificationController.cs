using Bug_Tracker.BL;
using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class TicketNotificationController : Controller
    {

        [Authorize(Roles = "developer")]
        public ActionResult Index()
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = UserService.GetUser(User.Identity.Name);
            else
                return new HttpUnauthorizedResult();

            var notifs = user.TicketNotifications.ToList();

            return View(notifs);
        }
    }
}