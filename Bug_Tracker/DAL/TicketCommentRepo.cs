using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketCommentRepo : IRepository<TicketComment>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(TicketComment entity)
        {
            db.TicketComments.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketComment> GetCollection(Func<TicketComment, bool> condition)
        {
            return db.TicketComments.Where(condition);
        }

        public TicketComment GetEntity(int id)
        {
            return db.TicketComments.Find(id);
        }

        public TicketComment GetEntity(Func<TicketComment, bool> condition)
        {
            return db.TicketComments.FirstOrDefault(condition);
        }

        public void Update(TicketComment entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}