using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketAttachmentRepo : IRepository<TicketAttachment>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Add(TicketAttachment entity)
        {
            db.TicketAttachments.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketAttachment> GetCollection(Func<TicketAttachment, bool> condition)
        {
            throw new NotImplementedException();
        }

        public TicketAttachment GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public TicketAttachment GetEntity(Func<TicketAttachment, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(TicketAttachment entity)
        {
            throw new NotImplementedException();
        }
    }
}