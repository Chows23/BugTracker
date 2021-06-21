﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketNotificationService
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public TicketNotificationRepo repo;
        public TicketNotificationService()
        {
            repo = new TicketNotificationRepo();
        }
        public TicketNotificationService(TicketNotificationRepo repo)
        {
            this.repo = repo;
        }


        public void Add(TicketNotification ticketNotification)
        {
            repo.Add(ticketNotification);
        }

        public TicketNotification Create(Ticket ticket, ApplicationUser user)
        {
            var notif = new TicketNotification()
            {
                TicketId = ticket.Id,
                UserId = user.Id,
            };

            return notif;
        }

        public TicketNotification GetTicketNotification(int ticketNotifId)
        {
            return repo.GetEntity(ticketNotifId);
        }

        public void RemoveNotif(TicketNotification notif)
        {
            repo.Delete(notif);
        }
    }
}