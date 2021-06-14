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
    }
}