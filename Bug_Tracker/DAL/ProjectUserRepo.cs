using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;

namespace Bug_Tracker.DAL
{
    public class ProjectUserRepo : IRepository<ProjectUser>
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        public void Add(ProjectUser entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProjectUser> GetCollection(Func<ProjectUser, bool> condition)
        {
            throw new NotImplementedException();
        }

        public ProjectUser GetEntity(int id)
        {
            throw new NotImplementedException();
        }

        public ProjectUser GetEntity(Func<ProjectUser, bool> condition)
        {
            throw new NotImplementedException();
        }

        public void Update(ProjectUser entity)
        {
            throw new NotImplementedException();
        }
    }
}