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

            var tickets = from s in db.Tickets
                          select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_asc":
                    tickets = tickets.OrderBy(s => s.Title);
                    break;
                case "name_desc":
                    tickets = tickets.OrderByDescending(s => s.Title);
                    break;
                case "date_asc":
                    tickets = tickets.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    tickets = tickets.OrderByDescending(s => s.Created);
                    break;
                case "desc_asc":
                    tickets = tickets.OrderBy(s => s.Description);
                    break;
                case "desc_desc":
                    tickets = tickets.OrderByDescending(s => s.Description);
                    break;
                case "proj_asc":
                    tickets = tickets.OrderBy(s => s.Project.Name);
                    break;
                case "proj_desc":
                    tickets = tickets.OrderByDescending(s => s.Project.Name);
                    break;
                case "prior_asc":
                    tickets = tickets.OrderBy(s => s.TicketPriority.Name);
                    break;
                case "prior_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketPriority.Name);
                    break;
                case "stat_asc":
                    tickets = tickets.OrderBy(s => s.TicketStatus.Name);
                    break;
                case "stat_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketStatus.Name);
                    break;
                case "type_asc":
                    tickets = tickets.OrderBy(s => s.TicketType.Name);
                    break;
                case "type_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketType.Name);
                    break;
                case "update_asc":
                    tickets = tickets.OrderBy(s => s.Updated);
                    break;
                case "update_desc":
                    tickets = tickets.OrderByDescending(s => s.Updated);
                    break;
                case "assign_asc":
                    tickets = tickets.OrderBy(s => s.AssignedToUser.UserName);
                    break;
                case "assign_desc":
                    tickets = tickets.OrderByDescending(s => s.AssignedToUser.UserName);
                    break;
                case "own_asc":
                    tickets = tickets.OrderBy(s => s.OwnerUser.UserName);
                    break;
                case "own_desc":
                    tickets = tickets.OrderByDescending(s => s.OwnerUser.UserName);
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