using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class ProjectRepo : IRepository<Project>
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Add(Project entity)
        {
            
        }

        public void Delete(Project entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Project> GetCollection(Func<Project, bool> condition)
        {
            throw new NotImplementedException();
        }

        public Project GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public Project GetEntity(Func<Project, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(Project entity)
        {
            throw new NotImplementedException();
        }
    }
}