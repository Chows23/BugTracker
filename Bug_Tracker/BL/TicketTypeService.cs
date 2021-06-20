﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketTypeService
    {
        private TicketTypeRepo repo;
        public TicketTypeService()
        {
            repo = new TicketTypeRepo();
        }
        public TicketTypeService(TicketTypeRepo repo)
        {
            this.repo = repo;
        }


        public void Create(TicketType ticketType)
        {
            repo.Add(ticketType);
        }
    }
}