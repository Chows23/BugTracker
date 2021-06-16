using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketHistoryService
    {
        public TicketHistoryRepo repo = new TicketHistoryRepo();

        public void Create(TicketHistory ticketHistory)
        {
            repo.Add(ticketHistory);
        }

        public void CompareTickets(Ticket oldTicket, Ticket newTicket)
        {
            if (oldTicket.Title != newTicket.Title)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Changed = DateTime.Now,
                    // add ids and stuff
                };

                Create(ticketHistory);
            }
        }
    }
}