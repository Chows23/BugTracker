using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketCommentService
    {
        public TicketCommentRepo repo = new TicketCommentRepo();
        public void Create(TicketComment ticketComment, Ticket ticket)
        {
            ticket.TicketComments.Add(ticketComment);
            repo.Add(ticketComment);
        }
    }
}