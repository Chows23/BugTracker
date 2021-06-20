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
        public TicketAttachmentService()
        {
            repo = new TicketAttachmentRepo();
        }
        public TicketAttachmentService(TicketAttachmentRepo repo)
        {
            this.repo = repo;
        }


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