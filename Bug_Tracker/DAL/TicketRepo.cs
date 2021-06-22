using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketRepo : IRepository<Ticket>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(Ticket entity)
        {
            entity.TicketStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "unresolved").Id;
            db.Tickets.Add(entity);
            db.SaveChanges();
        }

        public virtual IEnumerable<Ticket> GetCollection()
        {
            return db.Tickets;
        }

        public virtual IEnumerable<Ticket> GetCollection(Func<Ticket, bool> condition)
        {
            return db.Tickets.Where(condition).ToList();
        }

        public virtual IEnumerable<Ticket> GetCollection(Func<Ticket, DateTime> condition)
        {
            return db.Tickets.OrderByDescending(condition);
        }

        public virtual Ticket GetEntity(int id)
        {
            return db.Tickets.Find(id);
        }

        public Ticket GetEntity(Func<Ticket, bool> condition)
        {
            return db.Tickets.FirstOrDefault(condition);
        }

        public void Update(Ticket ticket)
        {
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
        }

        public virtual void Update(Ticket entity, ApplicationUser user)
        {
            if (user == null)
                entity.AssignedToUserId = null;
            else
                entity.AssignedToUserId = user.Id;
            db.SaveChanges();
        }
    }
}