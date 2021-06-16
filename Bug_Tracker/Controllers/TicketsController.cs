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

namespace Bug_Tracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketService ticketService = new TicketService();
        private TicketCommentService ticketCommentService = new TicketCommentService();

        // GET: Tickets
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tickets = from s in db.Tickets
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tickets = tickets.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    tickets = tickets.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    tickets = tickets.OrderByDescending(s => s.Created);
                    break;
                default:
                    tickets = tickets.OrderBy(s => s.Id);
                    break;
            }
            int pageSize = 5;
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
                // change updated time and create history object for each property changed
                // add assign to user
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
    }
}