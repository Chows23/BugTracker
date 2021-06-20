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
        private TicketPriorityRepo repo;
        public TicketPriorityService()
        {
            repo = new TicketPriorityRepo();
        }
        public TicketPriorityService(TicketPriorityRepo repo)
        {
            this.repo = repo;
        }


        public void Create(TicketPriority ticketPriority)
        {
            repo.Add(ticketPriority);
        }
    }
}