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
        ApplicationDbContext db = new ApplicationDbContext();
        public TicketHistoryRepo repo;
        public TicketHistoryService()
        {
            repo = new TicketHistoryRepo();
        }
        public TicketHistoryService(TicketHistoryRepo repo)
        {
            this.repo = repo;
        }


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
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var type = db.TicketTypes;

                var ticketHistory = new TicketHistory
                {
                    Property = "TicketType",
                    OldValue = type.FirstOrDefault(t=> t.Id == oldTicket.TicketTypeId).Name,
                    NewValue = type.FirstOrDefault(t => t.Id == newTicket.TicketTypeId).Name,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var priority = db.TicketPriorities;

                var ticketHistory = new TicketHistory
                {
                    Property = "TicketPriority",
                    OldValue = priority.FirstOrDefault(t => t.Id == oldTicket.TicketPriorityId).Name,
                    NewValue = priority.FirstOrDefault(t => t.Id == newTicket.TicketPriorityId).Name,
                    TicketId = oldTicket.Id,
                };
                historyList.Add(ticketHistory);
            }
            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var status = db.TicketStatuses;

                var ticketHistory = new TicketHistory
                {
                    Property = "TicketStatus",
                    OldValue = status.FirstOrDefault(t => t.Id == oldTicket.TicketStatusId).Name,
                    NewValue = status.FirstOrDefault(t => t.Id == newTicket.TicketStatusId).Name,
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