using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketPriorityRepo : IRepository<TicketPriority>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(TicketPriority entity)
        {
            db.TicketPriorities.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketPriority> GetCollection(Func<TicketPriority, bool> condition)
        {
            return db.TicketPriorities.Where(condition);
        }

        public TicketPriority GetEntity(int id)
        {
            return db.TicketPriorities.Find(id);
        }

        public TicketPriority GetEntity(Func<TicketPriority, bool> condition)
        {
            return db.TicketPriorities.FirstOrDefault(condition);
        }

        public void Update(TicketPriority entity)
        {
            throw new NotImplementedException();
        }
    }
}