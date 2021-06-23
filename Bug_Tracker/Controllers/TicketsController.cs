using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.Models;
using PagedList;
using Bug_Tracker.BL;
using System.IO;
using Microsoft.Ajax.Utilities;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketService ticketService = new TicketService();
        private TicketCommentService ticketCommentService = new TicketCommentService();
        private TicketHistoryService ticketHistoryService = new TicketHistoryService();
        private TicketAttachmentService ticketAttachmentService = new TicketAttachmentService();
        private ProjectUserService projectUserService = new ProjectUserService();
        private TicketNotificationService ticketNotificationService = new TicketNotificationService();

        // GET: Tickets      
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize, int? projectId, string ownerUserId, string assignedToUserId, string allTickets)
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = UserService.GetUser(User.Identity.Name);
            else
                return new HttpUnauthorizedResult();

            if (UserService.UserInRole(user.Id, "admin"))
                return RedirectToAction("AllTickets");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "name_desc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DescSortParm = sortOrder == "desc_asc" ? "desc_desc" : "desc_asc";
            ViewBag.ProjSortParm = sortOrder == "proj_asc" ? "proj_desc" : "proj_asc";
            ViewBag.PriorSortParm = sortOrder == "prior_asc" ? "prior_desc" : "prior_asc";
            ViewBag.StatSortParm = sortOrder == "stat_asc" ? "stat_desc" : "stat_asc";
            ViewBag.TypeSortParm = sortOrder == "type_asc" ? "type_desc" : "type_asc";
            ViewBag.UpdateSortParm = sortOrder == "update_asc" ? "update_desc" : "update_asc";
            ViewBag.AssignSortParm = sortOrder == "assign_asc" ? "assign_desc" : "assign_asc";
            ViewBag.OwnSortParm = sortOrder == "own_asc" ? "own_desc" : "own_asc";

            if (!string.IsNullOrEmpty(allTickets))
            {
                currentFilter = null;
                searchString = null;
                projectId = null;
                ownerUserId = null;
                assignedToUserId = null;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            searchString = searchString == null ? searchString : searchString.ToLower();

            ViewBag.CurrentFilter = searchString;

            var tickets = ticketService.GetFilteredTickets(searchString, user, projectId, ownerUserId, assignedToUserId);
            tickets = ticketService.GetSortedTickets(tickets, sortOrder);

            ViewBag.AssignedToUserId = new SelectList(tickets.DistinctBy(t => t.AssignedToUserId).Select(t => t.AssignedToUser).ToList(), "Id", "UserName");
            ViewBag.OwnerUserId = new SelectList(tickets.DistinctBy(t => t.OwnerUserId).Select(t => t.OwnerUser).ToList(), "Id", "UserName");
            ViewBag.ProjectId = new SelectList(tickets.DistinctBy(t => t.ProjectId).Select(t => t.Project).ToList(), "Id", "Name");

            if (pageSize == null)
                pageSize = 10;

            ViewBag.PageSize = pageSize;
            int pageNumber = (page ?? 1);

            ViewBag.Notifications = ticketNotificationService.GetNotifCount(user.Id);
            return View(tickets.ToPagedList(pageNumber, (int)pageSize));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult AllTickets(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize, int? projectId, string ownerUserId, string assignedToUserId, string allTickets)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "name_desc";
            ViewBag.DateSortParm = sortOrder == "date_asc" ? "date_desc" : "date_asc";
            ViewBag.DescSortParm = sortOrder == "desc_asc" ? "desc_desc" : "desc_asc";
            ViewBag.ProjSortParm = sortOrder == "proj_asc" ? "proj_desc" : "proj_asc";
            ViewBag.PriorSortParm = sortOrder == "prior_asc" ? "prior_desc" : "prior_asc";
            ViewBag.StatSortParm = sortOrder == "stat_asc" ? "stat_desc" : "stat_asc";
            ViewBag.TypeSortParm = sortOrder == "type_asc" ? "type_desc" : "type_asc";
            ViewBag.UpdateSortParm = sortOrder == "update_asc" ? "update_desc" : "update_asc";
            ViewBag.AssignSortParm = sortOrder == "assign_asc" ? "assign_desc" : "assign_asc";
            ViewBag.OwnSortParm = sortOrder == "own_asc" ? "own_desc" : "own_asc";

            if (!string.IsNullOrEmpty(allTickets))
            {
                currentFilter = null;
                searchString = null;
                projectId = null;
                ownerUserId = null;
                assignedToUserId = null;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            searchString = searchString == null ? searchString : searchString.ToLower();

            ViewBag.CurrentFilter = searchString;

            var tickets = ticketService.GetFilteredTickets(searchString, null, projectId, ownerUserId, assignedToUserId);
            tickets = ticketService.GetSortedTickets(tickets, sortOrder);

            ViewBag.AssignedToUserId = new SelectList(tickets.DistinctBy(t => t.AssignedToUserId).Select(t => t.AssignedToUser).ToList(), "Id", "UserName");
            ViewBag.OwnerUserId = new SelectList(tickets.DistinctBy(t => t.OwnerUserId).Select(t => t.OwnerUser).ToList(), "Id", "UserName");
            ViewBag.ProjectId = new SelectList(tickets.DistinctBy(t => t.ProjectId).Select(t => t.Project).ToList(), "Id", "Name");

            if (pageSize == null)
                pageSize = 10;

            ViewBag.PageSize = pageSize;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageNumber, (int)pageSize));
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }


            var user = UserService.GetUser(User.Identity.Name);
            if (user.Tickets.Contains(ticket))
            {

                ViewBag.Notifications = ticketNotificationService.GetNotifCount(user.Id);

                ViewBag.UserId = new SelectList(UserService.GetUserByRole("developer"), "Id", "UserName");
                return View(ticket);
            }
            else return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "submitter")]
        public ActionResult Create(int id)
        {

            ViewBag.ProjectId = id;
            ViewBag.Priority = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.Type = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "submitter")]
        public ActionResult Create([Bind(Include = "Title,Description,ProjectId,TicketTypeId,TicketPriorityId,OwnerUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticketService.Create(ticket);
                return RedirectToAction("Details", new { id = ticket.Id });
            }

            ViewBag.Priority = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.Type = new SelectList(db.TicketTypes, "Id", "Name");

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.Priority = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.Type = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.Status = new SelectList(db.TicketStatuses, "Id", "Name");
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,OwnerUserId,OwnerUser,AssignedToUserId,AssignedToUser,Created,Title,Description,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var oldTicket = ticketService.GetTicket(ticket.Id);
                ticket.Updated = DateTime.Now;

                if (oldTicket != null)
                {
                    ticketHistoryService.CompareTickets(oldTicket, ticket);
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (ticket.AssignedToUserId != null)
                {
                    var user = UserService.GetUserById(ticket.AssignedToUserId);
                    var newUserNotif = ticketNotificationService.Create(ticket, user);
                    ticketNotificationService.Add(newUserNotif);
                }

                return RedirectToAction("Details", new { id = ticket.Id });
            }

            ViewBag.Priority = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.Type = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.Status = new SelectList(db.TicketStatuses, "Id", "Name");
            return View(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Comment

        [HttpPost]
        public ActionResult Comment([Bind(Include = "Comment,TicketId,UserId")] TicketComment ticketComment)
        {
            var user = UserService.GetUser(User.Identity.Name);
            var ticket = ticketService.GetTicket(ticketComment.TicketId);

            // add comment to ticket in comment service

            if (ModelState.IsValid)
            {
                user.TicketComments.Add(ticketComment);
                ticketCommentService.Create(ticketComment, ticket);

                if (ticket.AssignedToUserId != null)
                {
                    var newUserNotif = ticketNotificationService.Create(ticket, user);
                    ticketNotificationService.Add(newUserNotif);
                }
            }
            else
                TempData["Error"] = "Your comment needs content.";

            return RedirectToAction("Details", "Tickets", new { id = ticketComment.TicketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Attach(int ticketId, HttpPostedFileBase file, string attachmentDescription)
        {
            string path;
            var user = UserService.GetUser(User.Identity.Name);
            var ticket = ticketService.GetTicket(ticketId);

            if (file != null && file.ContentLength > 0)
            {
                var fileName = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace(":", "-") + "_" + Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("../Data/attachments"), fileName);
                file.SaveAs(path);

                var newTicketAttachment = ticketAttachmentService.TicketAttachment(ticketId, fileName, attachmentDescription, user.Id, path);
                ticketAttachmentService.Create(newTicketAttachment);

                if (ticket.AssignedToUserId != null)
                {
                    var newUserNotif = ticketNotificationService.Create(ticket, user);
                    ticketNotificationService.Add(newUserNotif);
                }
            }

            return RedirectToAction("Details", new { id = ticketId });
        }

        public FileResult DownloadFile(string fileUrl, string filePath)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(fileUrl);
            string contentType = MimeMapping.GetMimeMapping(fileUrl);
            return File(bytes, contentType, filePath);
        }

        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        public ActionResult AssignDeveloper(int? id, string userId)
        {
            if (id == null || userId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = UserService.GetUserById(userId);
            var ticket = ticketService.GetTicket((int)id);

            if (user == null || ticket == null)
                return HttpNotFound();

            if (ModelState.IsValid && ticket.AssignedToUserId != userId)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "AssignedToUser",
                    OldValue = ticket.AssignedToUserId != null ? ticket.AssignedToUser.UserName : "Unassigned",
                    NewValue = user.UserName,
                    TicketId = ticket.Id
                };

                if (!projectUserService.CheckIfUserOnProject(ticket.ProjectId, user.Id))
                {
                    var newProjectUser = projectUserService.ProjectUser(user.Id, ticket.ProjectId);
                    projectUserService.Create(newProjectUser);
                }

                var newUserNotif = ticketNotificationService.Create(ticket, user);
                ticketNotificationService.Add(newUserNotif);

                ticketService.ChangeDeveloper(ticket, user);
                ticketHistoryService.Create(ticketHistory);
            }

            return RedirectToAction("Details", new { id = ticket.Id });
        }
    }
}