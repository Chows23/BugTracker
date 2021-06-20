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
        public TicketCommentRepo repo;
        public TicketCommentService()
        {
            repo = new TicketCommentRepo();
        }
        public TicketCommentService(TicketCommentRepo repo)
        {
            this.repo = repo;
        }


        public void Create(TicketComment ticketComment, Ticket ticket)
        {
            ticket.TicketComments.Add(ticketComment);
            repo.Add(ticketComment);
        }
    }
}