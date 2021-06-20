using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public TicketHistory GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public TicketHistory GetEntity(Func<TicketHistory, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(TicketHistory entity)
        {
            throw new NotImplementedException();
        }
    }
}