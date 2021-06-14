namespace Bug_Tracker.Migrations
{
    using Bug_Tracker.BL;
    using Bug_Tracker.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bug_Tracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Bug_Tracker.Models.ApplicationDbContext db)
        {
            // Roles

            if (db.Roles.Count() == 0)
            {
                UserService.AddRole("admin");
                UserService.AddRole("manager");
                UserService.AddRole("developer");
                UserService.AddRole("submitter");
            }

            // Users

            if (db.Users.Count() == 0)
            {
                var people = new Dictionary<string, string> {
                {"manager@gmail.com", "manager" },
                {"manager2@gmail.com", "manager" },
                {"chows@gmail.com", "developer" },
                {"elizabeth@gmail.com", "developer" },
                {"katherine@gmail.com", "developer" },
                {"admin@gmail.com", "admin" },
                {"admin2@gmail.com", "admin" },
                {"submitter@gmail.com", "submitter" },
                {"submitter2@gmail.com", "submitter" },
                };

                foreach (var person in people)
                {
                    var user = new ApplicationUser
                    {
                        Email = person.Key,
                        UserName = person.Key
                    };

                    UserService.CreateUser(user, "P3nguin!");

                    UserService.AddUserToRole(user.Id, person.Value);
                }
            }

            // Projects

            if (db.Projects.Count() == 0)
            {
                Project project1 = new Project
                {
                    Name = "Test Project 1",
                };
                Project project2 = new Project
                {
                    Name = "Test Project 2",
                };
                Project project3 = new Project
                {
                    Name = "Test Project 3",
                };
                Project project4 = new Project
                {
                    Name = "Test Project 4",
                };
                Project project5 = new Project
                {
                    Name = "Test Project 5",
                };

                db.Projects.AddRange(new List<Project>
                {
                    project1,
                    project2,
                    project3,
                    project4,
                    project5
                });
            }
        }
    }
}
