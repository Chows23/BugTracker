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
                        UserName = person.Key.Substring(0, person.Key.IndexOf('@')),
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

                db.SaveChanges();

                // Tickets

                TicketPriority highPriority = new TicketPriority
                {
                    Name = "High",
                };
                TicketPriority lowPriority = new TicketPriority
                {
                    Name = "Low",
                };

                TicketStatus unresolvedStatus = new TicketStatus
                {
                    Name = "Unresolved",
                };
                TicketStatus abandonedStatus = new TicketStatus
                {
                    Name = "Abandoned",
                };
                TicketStatus resolvedStatus = new TicketStatus
                {
                    Name = "Resolved",
                };

                TicketType bugType = new TicketType
                {
                    Name = "Bug",
                };
                TicketType featureType = new TicketType
                {
                    Name = "Feature",
                };
                TicketType functionType = new TicketType
                {
                    Name = "Function Change",
                };

                Ticket ticket1 = new Ticket
                {
                    Title = "Test Ticket 1",
                    Description = "This is a test bug ticket.",
                    Project = project1,
                    TicketType = bugType,
                    TicketPriority = highPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com"),
                };
                Ticket ticket2 = new Ticket
                {
                    Title = "Test Ticket 2",
                    Description = "This is a test feature ticket.",
                    Project = project1,
                    TicketType = featureType,
                    TicketPriority = lowPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "elizabeth@gmail.com"),
                };
                Ticket ticket3 = new Ticket
                {
                    Title = "Test Ticket 3",
                    Description = "This is a test function change ticket.",
                    Project = project2,
                    TicketType = functionType,
                    TicketPriority = highPriority,
                    TicketStatus = resolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "chows@gmail.com"),
                };
                Ticket ticket4 = new Ticket
                {
                    Title = "Test Ticket 4",
                    Description = "This is a test bug ticket.",
                    Project = project3,
                    TicketType = bugType,
                    TicketPriority = lowPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "elizabeth@gmail.com"),
                };
                Ticket ticket5 = new Ticket
                {
                    Title = "Test Ticket 5",
                    Description = "This is a test bug ticket.",
                    Project = project4,
                    TicketType = bugType,
                    TicketPriority = highPriority,
                    TicketStatus = resolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com"),
                };

                db.Tickets.AddRange(new List<Ticket>
                {
                    ticket1,
                    ticket2,
                    ticket3,
                    ticket4,
                    ticket5,
                });

                var katherine = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com");
                var chows = db.Users.FirstOrDefault(u => u.Email == "chows@gmail.com");
                var elizabeth = db.Users.FirstOrDefault(u => u.Email == "elizabeth@gmail.com");

                katherine.Tickets.Add(ticket1);
                katherine.Tickets.Add(ticket5);
                chows.Tickets.Add(ticket3);
                elizabeth.Tickets.Add(ticket2);
                elizabeth.Tickets.Add(ticket4);

                db.SaveChanges();
            }

            // ProjectUsers

            if (db.ProjectUsers.Count() == 0)
            {
                ProjectUser projectUser1 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 1"),
                    User = db.Users.First(u => u.Email == "manager@gmail.com")
                };
                ProjectUser projectUser2 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 2"),
                    User = db.Users.First(u => u.Email == "manager@gmail.com")
                };
                ProjectUser projectUser3 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 3"),
                    User = db.Users.First(u => u.Email == "manager@gmail.com")
                };
                ProjectUser projectUser4 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 4"),
                    User = db.Users.First(u => u.Email == "manager2@gmail.com")
                };
                ProjectUser projectUser5 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 5"),
                    User = db.Users.First(u => u.Email == "manager2@gmail.com")
                };
                ProjectUser projectUser6 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 1"),
                    User = db.Users.First(u => u.Email == "elizabeth@gmail.com")
                };
                ProjectUser projectUser7 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 2"),
                    User = db.Users.First(u => u.Email == "chows@gmail.com")
                };
                ProjectUser projectUser8 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 3"),
                    User = db.Users.First(u => u.Email == "katherine@gmail.com")
                };
                ProjectUser projectUser9 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 1"),
                    User = db.Users.First(u => u.Email == "katherine@gmail.com")
                };

                db.ProjectUsers.AddRange(new List<ProjectUser>
                {
                    projectUser1,
                    projectUser2,
                    projectUser3,
                    projectUser4,
                    projectUser5,
                    projectUser6,
                    projectUser7,
                    projectUser8,
                    projectUser9
                });

                db.SaveChanges();
            }
        }
    }
}