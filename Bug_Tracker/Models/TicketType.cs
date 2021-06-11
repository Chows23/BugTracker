﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}