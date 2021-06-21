namespace Bug_Tracker.Migrations
{
    using Bug_Tracker.BL;
    using Bug_Tracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Bug_Tracker.Models.ApplicationDbContext>
    {
        Random rand;
        UserManager<ApplicationUser> userManager;

        // Roles
        const string ROLE_ADMIN = "admin";
        const string ROLE_MANAGER = "manager";
        const string ROLE_DEVELOPER = "developer";
        const string ROLE_SUBMITTER = "submitter";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            this.rand = new Random();
        }

        protected override void Seed(Bug_Tracker.Models.ApplicationDbContext db)
        {
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            // Roles

            if (db.Roles.Count() == 0)
            {
                UserService.AddRole(ROLE_ADMIN);
                UserService.AddRole(ROLE_MANAGER);
                UserService.AddRole(ROLE_DEVELOPER);
                UserService.AddRole(ROLE_SUBMITTER);
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
                {"anton@gmail.com", "developer" },
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

                db.TicketStatuses.AddRange(new List<TicketStatus>
                {
                    unresolvedStatus,
                    abandonedStatus,
                    resolvedStatus,
                });

                db.TicketTypes.AddRange(new List<TicketType>
                {
                    bugType,
                    featureType,
                    functionType,
                });

                db.TicketPriorities.AddRange(new List<TicketPriority>
                {
                    highPriority,
                    lowPriority,
                });

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
                    Created = DateTime.Now.AddDays(-10),
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
                    Created = DateTime.Now.AddDays(-20),
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
                Ticket ticket6 = new Ticket
                {
                    Title = "Test Ticket 6",
                    Description = "This is a test feature ticket.",
                    Project = project5,
                    TicketType = featureType,
                    TicketPriority = lowPriority,
                    TicketStatus = abandonedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "chows@gmail.com"),
                    Created = DateTime.Now.AddDays(-15),
                };
                Ticket ticket7 = new Ticket
                {
                    Title = "Test Ticket 7",
                    Description = "This is a test bug ticket.",
                    Project = project5,
                    TicketType = bugType,
                    TicketPriority = highPriority,
                    TicketStatus = resolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "elizabeth@gmail.com"),
                    Created = DateTime.Now.AddDays(-11),
                };
                Ticket ticket8 = new Ticket
                {
                    Title = "Test Ticket 8",
                    Description = "This is a test function change ticket.",
                    Project = project4,
                    TicketType = functionType,
                    TicketPriority = lowPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "chows@gmail.com"),
                    Created = DateTime.Now.AddDays(-5),
                };
                Ticket ticket9 = new Ticket
                {
                    Title = "Test Ticket 9",
                    Description = "This is a test bug ticket.",
                    Project = project1,
                    TicketType = bugType,
                    TicketPriority = lowPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com"),
                    Created = DateTime.Now.AddDays(-25),
                };
                Ticket ticket10 = new Ticket
                {
                    Title = "Test Ticket 10",
                    Description = "This is a test feature ticket.",
                    Project = project2,
                    TicketType = featureType,
                    TicketPriority = lowPriority,
                    TicketStatus = resolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "elizabeth@gmail.com"),
                    Created = DateTime.Now.AddDays(-1),
                };
                Ticket ticket11 = new Ticket
                {
                    Title = "Test Ticket 11",
                    Description = "This is a test bug ticket.",
                    Project = project2,
                    TicketType = bugType,
                    TicketPriority = lowPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "chows@gmail.com"),
                    Created = DateTime.Now.AddDays(-15),
                };
                Ticket ticket12 = new Ticket
                {
                    Title = "Test Ticket 12",
                    Description = "This is a test function change ticket.",
                    Project = project1,
                    TicketType = functionType,
                    TicketPriority = lowPriority,
                    TicketStatus = abandonedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com"),
                    Created = DateTime.Now.AddDays(-45),
                };
                Ticket ticket13 = new Ticket
                {
                    Title = "Test Ticket 13",
                    Description = "This is a test bug ticket.",
                    Project = project1,
                    TicketType = bugType,
                    TicketPriority = lowPriority,
                    TicketStatus = unresolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "anton@gmail.com"),
                    Created = DateTime.Now.AddDays(-20),
                };
                Ticket ticket14 = new Ticket
                {
                    Title = "Test Ticket 14",
                    Description = "This is a test function change ticket.",
                    Project = project2,
                    TicketType = functionType,
                    TicketPriority = highPriority,
                    TicketStatus = resolvedStatus,
                    OwnerUser = db.Users.FirstOrDefault(u => u.Email == "submitter@gmail.com"),
                    AssignedToUser = db.Users.FirstOrDefault(u => u.Email == "anton@gmail.com"),
                    Created = DateTime.Now.AddDays(-10),
                };

                db.Tickets.AddRange(new List<Ticket>
                {
                    ticket1,
                    ticket2,
                    ticket3,
                    ticket4,
                    ticket5,
                    ticket6,
                    ticket7,
                    ticket8,
                    ticket9,
                    ticket10,
                    ticket11,
                    ticket12,
                    ticket13,
                    ticket14
                });

                db.SaveChanges();
            }

            // Ticket Attachments

            if (db.TicketAttachments.Count() == 0)
            {
                TicketAttachment ticketAttachment1 = new TicketAttachment
                {
                    Description = "This is a sample attachment.",
                    FilePath = "sample-1.txt",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-1.txt",
                    TicketId = 1,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment2 = new TicketAttachment
                {
                    Description = "This is a sample attachment.",
                    FilePath = "sample-2.txt",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-2.txt",
                    TicketId = 2,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment3 = new TicketAttachment
                {
                    Description = "This is a sample attachment.",
                    FilePath = "sample-3.txt",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-3.txt",
                    TicketId = 3,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment4 = new TicketAttachment
                {
                    Description = "This is a sample attachment.",
                    FilePath = "sample-4.txt",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-4.txt",
                    TicketId = 4,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment5 = new TicketAttachment
                {
                    Description = "This is a sample attachment.",
                    FilePath = "sample-5.txt",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-5.txt",
                    TicketId = 5,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment6 = new TicketAttachment
                {
                    Description = "This is a sample photo attachment.",
                    FilePath = "sample-photo-1.jpeg",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-photo-1.jpeg",
                    TicketId = 1,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment7 = new TicketAttachment
                {
                    Description = "This is a sample photo attachment.",
                    FilePath = "sample-photo-2.jpeg",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-photo-2.jpeg",
                    TicketId = 2,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment8 = new TicketAttachment
                {
                    Description = "This is a sample photo attachment.",
                    FilePath = "sample-photo-3.jpeg",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-photo-3.jpeg",
                    TicketId = 3,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment9 = new TicketAttachment
                {
                    Description = "This is a sample photo attachment.",
                    FilePath = "sample-photo-4.jpeg",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-photo-4.jpeg",
                    TicketId = 4,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };
                TicketAttachment ticketAttachment10 = new TicketAttachment
                {
                    Description = "This is a sample photo attachment.",
                    FilePath = "sample-photo-5.jpeg",
                    FileUrl = @"C:\Users\kathe\Desktop\BugTracker\BugTracker\Bug_Tracker\Data\attachments\sample-photo-5.jpeg",
                    TicketId = 5,
                    UserId = db.Users.FirstOrDefault(u => u.Email == "katherine@gmail.com").Id,
                };

                db.TicketAttachments.AddRange(new List<TicketAttachment>
                {
                    ticketAttachment1,
                    ticketAttachment2,
                    ticketAttachment3,
                    ticketAttachment4,
                    ticketAttachment5,
                    ticketAttachment6,
                    ticketAttachment7,
                    ticketAttachment8,
                    ticketAttachment9,
                    ticketAttachment10,
                });
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
                ProjectUser projectUser10 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 1"),
                    User = db.Users.First(u => u.Email == "submitter@gmail.com")
                };
                ProjectUser projectUser11 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 2"),
                    User = db.Users.First(u => u.Email == "submitter@gmail.com")
                };
                ProjectUser projectUser12 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 3"),
                    User = db.Users.First(u => u.Email == "submitter@gmail.com")
                };
                ProjectUser projectUser13 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 4"),
                    User = db.Users.First(u => u.Email == "submitter@gmail.com")
                };
                ProjectUser projectUser14 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 5"),
                    User = db.Users.First(u => u.Email == "submitter@gmail.com")
                };
                ProjectUser projectUser15 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 3"),
                    User = db.Users.First(u => u.Email == "elizabeth@gmail.com")
                };
                ProjectUser projectUser16 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 4"),
                    User = db.Users.First(u => u.Email == "katherine@gmail.com")
                };
                ProjectUser projectUser17 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 5"),
                    User = db.Users.First(u => u.Email == "chows@gmail.com")
                };
                ProjectUser projectUser18 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 5"),
                    User = db.Users.First(u => u.Email == "elizabeth@gmail.com")
                };
                ProjectUser projectUser19 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 4"),
                    User = db.Users.First(u => u.Email == "chows@gmail.com")
                };
                ProjectUser projectUser20 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 2"),
                    User = db.Users.First(u => u.Email == "elizabeth@gmail.com")
                };
                ProjectUser projectUser21 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 1"),
                    User = db.Users.First(u => u.Email == "anton@gmail.com")
                };
                ProjectUser projectUser22 = new ProjectUser
                {
                    Project = db.Projects.First(p => p.Name == "Test Project 2"),
                    User = db.Users.First(u => u.Email == "anton@gmail.com")
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
                    projectUser9,
                    projectUser10,
                    projectUser11,
                    projectUser12,
                    projectUser13,
                    projectUser14,
                    projectUser15,
                    projectUser16,
                    projectUser17,
                    projectUser18,
                    projectUser19,
                    projectUser20,
                    projectUser21,
                    projectUser22,
                });

                db.SaveChanges();


            }

            // TicketComments

            int numOfComments = 200;
            if (db.TicketComments.Count() < numOfComments)
            {
                List<ApplicationUser> users = db.Users.ToList();
                List<Ticket> tickets = db.Tickets.ToList();

                List<TicketComment> comments = new List<TicketComment>();

                for (int i = 0; i < numOfComments; i++)
                {
                    ApplicationUser user = users[rand.Next(users.Count)];
                    Ticket ticket = new Ticket();
                    if (userManager.IsInRole(user.Id, ROLE_ADMIN))
                    {
                        ticket = tickets[rand.Next(tickets.Count)];
                    }
                    else if (userManager.IsInRole(user.Id, ROLE_MANAGER))
                    {
                        Project project = user.ProjectUsers
                            .Skip(rand.Next(user.ProjectUsers.Count))
                            .FirstOrDefault().Project;
                        ticket = project.Tickets.ToList()[rand.Next(project.Tickets.Count)];
                    }
                    else if (userManager.IsInRole(user.Id, ROLE_DEVELOPER))
                    {
                        ticket = tickets.Where(t => t.AssignedToUserId == user.Id)
                            .Skip(rand.Next(tickets.Where(t => t.AssignedToUserId == user.Id).Count()))
                            .FirstOrDefault();
                    }
                    else if (userManager.IsInRole(user.Id, ROLE_SUBMITTER))
                    {
                        ticket = tickets.Where(t => t.OwnerUserId == user.Id)
                            .Skip(rand.Next(tickets.Where(t => t.OwnerUserId == user.Id).Count()))
                            .FirstOrDefault();
                    }
                    if (ticket != null && ticket.OwnerUserId != null)
                        comments.Add(new TicketComment
                        {
                            User = user,
                            Ticket = ticket,
                            Comment = Faker.Lorem.Paragraph(),
                            Created = ticket.Created.AddDays(rand.Next(5))
                        });
                }
                db.TicketComments.AddRange(comments);
                db.SaveChanges();
            }

            // TicketHistory

            if (db.TicketHistories.Count() < db.Tickets.Count())
            {
                List<Ticket> tickets = db.Tickets.ToList();
                List<TicketHistory> histories = new List<TicketHistory>();
                foreach (Ticket ticket in tickets)
                {
                    if (Faker.Boolean.Random())
                    {
                        histories.Add(new TicketHistory
                        {
                            Property = "Title",
                            OldValue = Faker.Company.CatchPhrase(),
                            NewValue = ticket.Title,
                            Changed = ticket.Updated,
                            TicketId = ticket.Id,
                            UserId = ticket.AssignedToUserId
                        });
                    }
                    if (Faker.Boolean.Random())
                    {
                        histories.Add(new TicketHistory
                        {
                            Property = "Description",
                            OldValue = Faker.Lorem.Paragraph(),
                            NewValue = ticket.Description,
                            Changed = ticket.Updated,
                            TicketId = ticket.Id,
                            UserId = ticket.AssignedToUserId
                        });
                    }
                    if (Faker.Boolean.Random())
                    {
                        List<TicketType> types = db.TicketTypes.Where(t => t.Id != ticket.TicketTypeId).ToList();
                        histories.Add(new TicketHistory
                        {
                            Property = "TicketType",
                            OldValue = types[rand.Next(types.Count)].Name,
                            NewValue = ticket.TicketType.Name,
                            Changed = ticket.Updated,
                            TicketId = ticket.Id,
                            UserId = ticket.AssignedToUserId
                        });
                    }
                    if (Faker.Boolean.Random())
                    {
                        List<TicketPriority> priorities = db.TicketPriorities.Where(p => p.Id != ticket.TicketPriorityId).ToList();
                        histories.Add(new TicketHistory
                        {
                            Property = "TicketPriority",
                            OldValue = priorities[rand.Next(priorities.Count)].Name,
                            NewValue = ticket.TicketPriority.Name,
                            Changed = ticket.Updated,
                            TicketId = ticket.Id,
                            UserId = ticket.AssignedToUserId
                        });
                    }
                    if (Faker.Boolean.Random())
                    {
                        List<TicketStatus> statuses = db.TicketStatuses.Where(p => p.Id != ticket.TicketStatusId).ToList();

                        histories.Add(new TicketHistory
                        {
                            Property = "TicketStatus",
                            OldValue = statuses[rand.Next(statuses.Count)].Name,
                            NewValue = ticket.TicketStatus.Name,
                            Changed = ticket.Updated,
                            TicketId = ticket.Id,
                            UserId = ticket.AssignedToUserId
                        });
                    }
                }
                db.TicketHistories.AddRange(histories);
                db.SaveChanges();
            }

            // TicketNotifications

            if (db.TicketNotifications.Count() < db.Tickets.Count())
            {
                List<Ticket> tickets = db.Tickets.ToList();
                List<TicketNotification> notifications = new List<TicketNotification>();
                foreach (Ticket ticket in tickets)
                {
                    notifications.Add(new TicketNotification
                    {
                        TicketId = ticket.Id,
                        UserId = ticket.AssignedToUserId
                    });
                }
                List<TicketHistory> histories = db.TicketHistories.ToList();
                foreach (TicketHistory history in histories)
                {
                    notifications.Add(new TicketNotification
                    {
                        TicketId = history.TicketId,
                        UserId = history.Ticket.AssignedToUserId
                    });
                }
                db.TicketNotifications.AddRange(notifications);
                db.SaveChanges();
            }
        }
    }
}