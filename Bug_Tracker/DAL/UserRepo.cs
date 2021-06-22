using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Bug_Tracker.DAL
{
    public class UserRepo : IRepository<ApplicationUser>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Add(ApplicationUser entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetCollection(Func<ApplicationUser, bool> condition)
        {
            return db.Users.Where(condition);
        }

        public IEnumerable<ApplicationUser> GetCollection()
        {
            return db.Users;
        }

        public ApplicationUser GetEntity(int id)
        {
            return db.Users.Find(id);
        }

        public ApplicationUser GetEntity(Func<ApplicationUser, bool> condition)
        {
            return db.Users.FirstOrDefault(condition);
        }

        public void Update(ApplicationUser entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}