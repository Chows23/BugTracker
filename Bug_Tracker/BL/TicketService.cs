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

        public IEnumerable<Ticket> GetFilteredTickets(string searchString, ApplicationUser user)
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
                tickets = user.SubmittedTickets;
            }
            else
                tickets = user.Tickets;

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.Title.Contains(searchString)
                                       || s.Description.Contains(searchString));
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
                return user.Tickets.OrderByDescending(t => t.Updated).Take(n).ToList();
        }

        public List<Ticket> GetNLatestCreated(int n, ApplicationUser user)
        {
            if (UserService.UserInRole(user.Id, "submitter"))
                return user.SubmittedTickets.OrderByDescending(t => t.Created).Take(n).ToList();
            else
                return user.Tickets.OrderByDescending(t => t.Created).Take(n).ToList();
        }

        public void ChangeDeveloper(Ticket ticket, ApplicationUser user)
        {
            repo.Update(ticket, user);
        }
    }
}