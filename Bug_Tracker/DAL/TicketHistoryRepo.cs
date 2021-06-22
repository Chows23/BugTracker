using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketHistoryRepo : IRepository<TicketHistory>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(TicketHistory entity)
        {
            db.TicketHistories.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketHistory> GetCollection(Func<TicketHistory, bool> condition)
        {
            return db.TicketHistories.Where(condition);
        }

        public TicketHistory GetEntity(int id)
        {
            return db.TicketHistories.Find(id);
        }

        public TicketHistory GetEntity(Func<TicketHistory, bool> condition)
        {
            return db.TicketHistories.FirstOrDefault(condition);
        }

        public void Update(TicketHistory entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}