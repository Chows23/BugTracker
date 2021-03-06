using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;

namespace Bug_Tracker.BL
{
    public class TicketService
    {
        public TicketRepo repo;
        public TicketService()
        {
            repo = new TicketRepo();
        }
        public TicketService(TicketRepo repo)
        {
            this.repo = repo;
        }


        public void Create(Ticket ticket)
        {
            repo.Add(ticket);
        }

        public Ticket GetTicket(int ticketId)
        {
            return repo.GetEntity(ticketId);
        }

        public IEnumerable<Ticket> GetFilteredTickets(string searchString, ApplicationUser user, int? projectId, string ownerUserId, string assignedToUserId)
        {
            IEnumerable<Ticket> tickets;

            if (user == null)
                tickets = repo.GetCollection();
            else if (UserService.UserInRole(user.Id, "manager"))
            {
                tickets = repo.GetCollection().ToList();
                tickets = tickets.Where(t => t.Project.ProjectUsers.Any(pu => pu.UserId == user.Id));
            }
            else if (UserService.UserInRole(user.Id, "submitter"))
            {
                tickets = repo.GetCollection(t => t.OwnerUserId == user.Id);
            }
            else
                tickets = repo.GetCollection(t => t.AssignedToUserId == user.Id);

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.ToList().Where(t => t.Title.ToLower().Contains(searchString)
                                       || t.Description.ToLower().Contains(searchString)
                                       || t.Project.Name.ToLower().Contains(searchString)
                                       || t.TicketPriority.Name.ToLower().Contains(searchString)
                                       || t.TicketStatus.Name.ToLower().Contains(searchString)
                                       || t.TicketType.Name.ToLower().Contains(searchString)
                                       || t.Created.ToString().ToLower().Contains(searchString)
                                       || t.Updated.ToString().ToLower().Contains(searchString)
                                       || t.OwnerUser.UserName.ToLower().Contains(searchString));
            }

            if (projectId != null)
            {
                tickets = tickets.Where(t => t.ProjectId == projectId);
            }

            if (!String.IsNullOrEmpty(ownerUserId))
            {
                tickets = tickets.Where(t => t.OwnerUserId.Equals(ownerUserId));
            }

            if (!String.IsNullOrEmpty(assignedToUserId))
            {
                tickets = tickets.Where(t => t.AssignedToUserId.Equals(assignedToUserId));
            }

            return tickets;
        }

        public IEnumerable<Ticket> GetSortedTickets(IEnumerable<Ticket> tickets, string sortOrder)
        {
            switch (sortOrder)
            {
                case "name_asc":
                    tickets = tickets.OrderBy(s => s.Title);
                    break;
                case "name_desc":
                    tickets = tickets.OrderByDescending(s => s.Title);
                    break;
                case "date_asc":
                    tickets = tickets.OrderBy(s => s.Created);
                    break;
                case "date_desc":
                    tickets = tickets.OrderByDescending(s => s.Created);
                    break;
                case "desc_asc":
                    tickets = tickets.OrderBy(s => s.Description);
                    break;
                case "desc_desc":
                    tickets = tickets.OrderByDescending(s => s.Description);
                    break;
                case "proj_asc":
                    tickets = tickets.OrderBy(s => s.Project.Name);
                    break;
                case "proj_desc":
                    tickets = tickets.OrderByDescending(s => s.Project.Name);
                    break;
                case "prior_asc":
                    tickets = tickets.OrderBy(s => s.TicketPriority.Name);
                    break;
                case "prior_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketPriority.Name);
                    break;
                case "stat_asc":
                    tickets = tickets.OrderBy(s => s.TicketStatus.Name);
                    break;
                case "stat_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketStatus.Name);
                    break;
                case "type_asc":
                    tickets = tickets.OrderBy(s => s.TicketType.Name);
                    break;
                case "type_desc":
                    tickets = tickets.OrderByDescending(s => s.TicketType.Name);
                    break;
                case "update_asc":
                    tickets = tickets.OrderBy(s => s.Updated);
                    break;
                case "update_desc":
                    tickets = tickets.OrderByDescending(s => s.Updated);
                    break;
                case "assign_asc":
                    tickets = tickets.OrderBy(s => s.AssignedToUserId);
                    break;
                case "assign_desc":
                    tickets = tickets.OrderByDescending(s => s.AssignedToUserId);
                    break;
                case "own_asc":
                    tickets = tickets.OrderBy(s => s.OwnerUserId);
                    break;
                case "own_desc":
                    tickets = tickets.OrderByDescending(s => s.OwnerUserId);
                    break;
                default:
                    tickets = tickets.OrderBy(s => s.Id);
                    break;

            }

            return tickets;
        }

        public List<Ticket> GetNLatestUpdated(int n, ApplicationUser user)
        {
            if (user == null)
                return repo.GetCollection(t => t.Updated).Take(n).ToList();
            else
                return GetUserTickets(user.Id).OrderByDescending(t => t.Updated).Take(n).ToList();
        }

        public List<Ticket> GetNLatestCreated(int n, ApplicationUser user)
        {
            if (UserService.UserInRole(user.Id, "submitter"))
                return GetOwnerTickets(user.Id).OrderByDescending(t => t.Created).Take(n).ToList();
            else
                return repo.GetCollection().OrderByDescending(t => t.Created).Take(n).ToList();
        }

        public void ChangeDeveloper(Ticket ticket, ApplicationUser user)
        {
            repo.Update(ticket, user);
        }

        public void RemoveTicketUser(Ticket ticket)
        {
            repo.Update(ticket, null);
        }

        public void UnassignUserTickets(int projectId, string userId)
        {
            var tickets = repo.GetCollection(t => t.ProjectId == projectId && t.AssignedToUserId == userId);
            foreach (var ticket in tickets)
            {
                RemoveTicketUser(ticket);
            }
        }

        public List<Ticket> GetUserTickets(string userId)
        {
            if (userId == null)
                return repo.GetCollection(t => t.AssignedToUserId == null).ToList();
            else
                return repo.GetCollection(t => t.AssignedToUserId == userId).ToList();
        }

        public List<Ticket> GetOwnerTickets(string userId)
        {
            return repo.GetCollection(t => t.OwnerUserId == userId).ToList();
        }
    }
}