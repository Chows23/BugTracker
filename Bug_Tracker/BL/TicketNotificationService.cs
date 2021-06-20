using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketNotificationService
    {
        public TicketNotificationRepo repo;
        public TicketNotificationService()
        {
            repo = new TicketNotificationRepo();
        }
        public TicketNotificationService(TicketNotificationRepo repo)
        {
            this.repo = repo;
        }


        public void Create(TicketNotification ticketNotification)
        {
            repo.Add(ticketNotification);
        }
    }
}