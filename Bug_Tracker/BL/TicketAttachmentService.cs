using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.DAL;
using Bug_Tracker.Models;

namespace Bug_Tracker.BL
{
    public class TicketAttachmentService
    {
        public TicketAttachmentRepo repo;

        public void Create(TicketAttachment ticketAttachment)
        {
            repo.Add(ticketAttachment);
        }
    }
}