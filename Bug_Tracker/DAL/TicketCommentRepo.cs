using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public TicketComment GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public TicketComment GetEntity(Func<TicketComment, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(TicketComment entity)
        {
            throw new NotImplementedException();
        }
    }
}