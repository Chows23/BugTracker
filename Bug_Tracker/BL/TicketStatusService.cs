using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketStatusService
    {
        public TicketStatusRepo repo;
        public void Create(TicketStatus ticketStatus)
        {
            repo.Add(ticketStatus);
        }
    }
}