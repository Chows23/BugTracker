using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;

namespace Bug_Tracker.BL
{
    public class TicketPriorityService
    {
        private TicketPriorityRepo repo = new TicketPriorityRepo();
        public void Create(TicketPriority ticketPriority)
        {
            repo.Add(ticketPriority);
        }
    }
}