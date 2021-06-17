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
        public TicketAttachmentRepo repo = new TicketAttachmentRepo();

        public void Create(TicketAttachment ticketAttachment)
        {
            repo.Add(ticketAttachment);
        }

        public TicketAttachment TicketAttachment(int ticketId, string filePath, string description, string userId, string fileUrl)
        {
            var ticketAttachment = new TicketAttachment
            {
                TicketId = ticketId,
                FilePath = filePath,
                Description = description,
                UserId = userId,
                FileUrl = fileUrl
            };

            return ticketAttachment;
        }
    }
}