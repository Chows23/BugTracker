using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class ProjectUserRepo : IRepository<ProjectUser>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(ProjectUser entity)
        {
            db.ProjectUsers.Add(entity);
            db.SaveChanges();
        }

        public virtual IEnumerable<ProjectUser> GetCollection(Func<ProjectUser, bool> condition)
        {
            if (condition == null)
                return db.ProjectUsers;
            return db.ProjectUsers.Where(condition);
        }

        public virtual ProjectUser GetEntity(int id)
        {
            return db.ProjectUsers.Find(id);
        }

        public ProjectUser GetEntity(Func<ProjectUser, bool> condition)
        {
            return db.ProjectUsers.FirstOrDefault(condition);
        }

        public void Update(ProjectUser entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var projectUser = db.ProjectUsers.Find(id);
            db.ProjectUsers.Remove(projectUser);
            db.SaveChanges();
        }
    }
}