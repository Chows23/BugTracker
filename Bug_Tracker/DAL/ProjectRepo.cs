using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class ProjectRepo : IRepository<Project>
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void Add(Project entity)
        {
            db.Projects.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Project> GetCollection()
        {
            return db.Projects;
        }

        public IEnumerable<Project> GetCollection(Func<Project, bool> condition)
        {
            return db.Projects.Where(condition);
        }

        public IEnumerable<Project> GetCollection(Func<Project, DateTime> condition)
        {
            return db.Projects.OrderByDescending(condition);
        }

        public Project GetEntity(int id)
        {
            return db.Projects.Find(id);
        }

        public Project GetEntity(Func<Project, bool> condition)
        {
            return db.Projects.FirstOrDefault(condition);
        }

        public void Update(Project entity)
        {
            throw new NotImplementedException();
        }
    }
}