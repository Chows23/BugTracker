using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketRepo : IRepository<Ticket>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Add(Ticket entity)
        {
            entity.TicketStatusId = db.TicketStatuses.FirstOrDefault(ts => ts.Name == "unresolved").Id;
            db.Tickets.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Ticket> GetCollection()
        {
            return db.Tickets;
        }

        public IEnumerable<Ticket> GetCollection(Func<Ticket, bool> condition)
        {
            return db.Tickets.Where(condition).ToList();
        }

        public IEnumerable<Ticket> GetCollection(Func<Ticket, DateTime> condition)
        {
            return db.Tickets.OrderByDescending(condition);
        }

        public Ticket GetEntity(int id)
        {
            return db.Tickets.Find(id);
        }

        public Ticket GetEntity(Func<Ticket, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public void Update(Ticket entity, ApplicationUser user)
        {
            entity.AssignedToUserId = user.Id;
            db.SaveChanges();
        }
    }
}