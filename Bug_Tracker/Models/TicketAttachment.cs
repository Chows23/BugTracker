using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class TicketAttachment
    {
        public TicketAttachment()
        {
            this.Created = DateTime.Now;
        }
        public int Id { get; set; }
        public int TicketId { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        [Required]
        public string FileUrl { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}