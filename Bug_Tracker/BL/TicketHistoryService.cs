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
            var historyList = new List<TicketHistory>();
            if (oldTicket.Title != newTicket.Title)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    Changed = DateTime.Now,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.Description != newTicket.Description)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "Description",
                    OldValue = oldTicket.Description,
                    NewValue = newTicket.Description,
                    Changed = DateTime.Now,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.TicketType != newTicket.TicketType)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "TicketType",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    Changed = DateTime.Now,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.TicketPriority != newTicket.TicketPriority)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "TicketPriority",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    Changed = DateTime.Now,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.TicketStatus != newTicket.TicketStatus)
            {
                var ticketHistory = new TicketHistory
                {
                    Property = "TicketStatus",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    Changed = DateTime.Now,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            foreach (var history in historyList)
            {
                Create(history);
            }
        }

    }
}