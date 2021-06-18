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

        // GET: Tickets      
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tickets = ticketService.GetFilteredTickets(searchString, user);
            tickets = ticketService.GetSortedTickets(tickets, sortOrder);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult AllTickets(string sortOrder, string currentFilter, string searchString, int? page)
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

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tickets = ticketService.GetFilteredTickets(searchString, null);
            tickets = ticketService.GetSortedTickets(tickets, sortOrder);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(tickets.ToPagedList(pageNumber, pageSize));
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
            ViewBag.UserId = new SelectList(UserService.GetUserByRole("developer"), "Id", "UserName");
            return View(ticket);
        }

        // GET: Tickets/Create
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
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProjectId,OwnerUserId,OwnerUser,Created,Title,Description,TicketTypeId,TicketPriorityId,TicketStatusId")] Ticket ticket)
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

                return RedirectToAction("Details", new { id = ticket.Id });
            }

            ViewBag.Priority = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.Type = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.Status = new SelectList(db.TicketStatuses, "Id", "Name");
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
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
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            
            if (file != null && file.ContentLength > 0)
            {
                var fileName = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss").Replace(":", "-") + "_" + Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("../Data/attachments"), fileName);
                file.SaveAs(path);

                var newTicketAttachment = ticketAttachmentService.TicketAttachment(ticketId, file.FileName, attachmentDescription, user.Id, path);
                ticketAttachmentService.Create(newTicketAttachment);
            }
            
            return RedirectToAction("Details", new { id = ticketId });
        }

        [HttpPost]
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
                ticketService.ChangeDeveloper(ticket, user);
                ticketHistoryService.Create(ticketHistory);
            }

            return RedirectToAction("Details", new { id = ticket.Id });
        }
    }
}