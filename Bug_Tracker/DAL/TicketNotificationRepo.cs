using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketNotificationRepo : IRepository<TicketNotification>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Add(TicketNotification entity)
        {
            db.TicketNotifications.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketNotification> GetCollection(Func<TicketNotification, bool> condition)
        {
            throw new NotImplementedException();
        }

        public TicketNotification GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public TicketNotification GetEntity(Func<TicketNotification, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(TicketNotification entity)
        {
            throw new NotImplementedException();
        }
    }
}