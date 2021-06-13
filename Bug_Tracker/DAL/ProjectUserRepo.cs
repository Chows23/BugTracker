using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class ProjectUserRepo : IRepository<ProjectUser>
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public void Add(ProjectUser entity)
        {
            db.ProjectUsers.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<ProjectUser> GetCollection(Func<ProjectUser, bool> condition)
        {
            return db.ProjectUsers.Where(condition);
        }

        public ProjectUser GetEntity(int id)
        {
            return db.ProjectUsers.Find(id);
        }

        public ProjectUser GetEntity(Func<ProjectUser, bool> condition)
        {
            return db.ProjectUsers.FirstOrDefault(condition);
        }

        public void Update(ProjectUser entity)
        {
            throw new NotImplementedException();
        }
    }
}