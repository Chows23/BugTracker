using Bug_Tracker.BL;
using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    [Authorize(Roles = "developer")]
    public class TicketNotificationController : Controller
    {
        private TicketNotificationService ticketNotificationService = new TicketNotificationService();

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

        public ActionResult RemoveNotificationFromUser(int? notifId)
        {
            if (notifId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var notification = ticketNotificationService.GetTicketNotification((int)notifId);
            var user = UserService.GetUser(User.Identity.Name);

            //ticketNotificationService.RemoveNotifFromUser(user, notification);

            return RedirectToAction("Index");
        }
    }
}