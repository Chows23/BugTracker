using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Ticket
    {
        public Ticket()
        {
            this.TicketNotifications = new HashSet<TicketNotification>();
            this.TicketAttachments = new HashSet<TicketAttachments>();
            this.TicketComments = new HashSet<TicketComments>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.Created = DateTime.Now;
            this.Updated = Created;
        }
        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } 
        public DateTime Updated { get; set; }
        public int ProjectId { get; set; }
        public int TicketTypeId { get; set; }
        public int TicketPriorityId { get; set; }
        public int TicketStatusId { get; set; }
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }

        public virtual Project Project { get; set; }
        [Required]
        public virtual TicketType TicketType { get; set; }
        [Required]
        public virtual TicketPriority TicketPriority { get; set; }
        [Required]
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketComments> TicketComments { get; set; }
        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }
    }
}