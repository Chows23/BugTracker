using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.DAL
{
    public class UserRepo : IRepository<ApplicationUser>
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public ApplicationUser GetEntity(Func<ApplicationUser, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}