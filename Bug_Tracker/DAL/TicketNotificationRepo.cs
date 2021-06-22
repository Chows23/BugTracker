using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketNotificationRepo : IRepository<TicketNotification>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(TicketNotification entity)
        {
            db.TicketNotifications.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketNotification> GetCollection(Func<TicketNotification, bool> condition)
        {
            return db.TicketNotifications.Where(condition);
        }

        public TicketNotification GetEntity(int id)
        {
            return db.TicketNotifications.Find(id);
        }

        public TicketNotification GetEntity(Func<TicketNotification, bool> condition)
        {
            return db.TicketNotifications.FirstOrDefault(condition);
        }

        public void Update(TicketNotification entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(TicketNotification entity)
        {
            db.TicketNotifications.Remove(entity);
            db.SaveChanges();
        }
    }
}