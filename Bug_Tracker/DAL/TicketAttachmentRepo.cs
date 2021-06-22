using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketAttachmentRepo : IRepository<TicketAttachment>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(TicketAttachment entity)
        {
            db.TicketAttachments.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketAttachment> GetCollection(Func<TicketAttachment, bool> condition)
        {
            return db.TicketAttachments.Where(condition);
        }

        public TicketAttachment GetEntity(int id)
        {
            return db.TicketAttachments.Find(id);
        }

        public TicketAttachment GetEntity(Func<TicketAttachment, bool> condition)
        {
            return db.TicketAttachments.FirstOrDefault(condition);
        }

        public void Update(TicketAttachment entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}