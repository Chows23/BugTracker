using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class ProjectRepo : IRepository<Project>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public virtual void Add(Project entity)
        {
            db.Projects.Add(entity);
            db.SaveChanges();
        }

        public virtual IEnumerable<Project> GetCollection()
        {
            return db.Projects;
        }

        public IEnumerable<Project> GetCollection(Func<Project, bool> condition)
        {
            return db.Projects.Where(condition);
        }

        public virtual IEnumerable<Project> GetAll()
        {
            return db.Projects;
        }

        public IEnumerable<Project> GetCollection(Func<Project, DateTime> condition)
        {
            return db.Projects.OrderByDescending(condition);
        }

        public virtual Project GetEntity(int id)
        {
            return db.Projects.Find(id);
        }

        public Project GetEntity(Func<Project, bool> condition)
        {
            return db.Projects.FirstOrDefault(condition);
        }

        public virtual void Update(Project entity)
        {
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}