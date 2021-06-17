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
        public TicketRepo repo = new TicketRepo();

        public void Create(Ticket ticket)
        {
            repo.Add(ticket);
        }

        public void UpDate(Ticket oldTicket, Ticket newTicket)
        {
            oldTicket.Title = newTicket.Title;
            oldTicket.Description = newTicket.Description;
        }

        public Ticket GetTicket(int ticketId)
        {
           return repo.GetEntity(ticketId);
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
            return user.Tickets.OrderByDescending(t => t.Created).Take(n).ToList();
        }
    }
}