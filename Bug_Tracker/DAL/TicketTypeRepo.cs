using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class TicketTypeRepo : IRepository<TicketType>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Add(TicketType entity)
        {
            db.TicketTypes.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<TicketType> GetCollection(Func<TicketType, bool> condition)
        {
            return db.TicketTypes.Where(condition);
        }

        public TicketType GetEntity(int id)
        {
            return db.TicketTypes.Find(id);
        }

        public TicketType GetEntity(Func<TicketType, bool> condition)
        {
            return db.TicketTypes.FirstOrDefault(condition);
        }

        public void Update(TicketType entity)
        {
            throw new NotImplementedException();
        }
    }
}