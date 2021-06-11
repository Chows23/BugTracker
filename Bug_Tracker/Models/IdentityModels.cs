using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bug_Tracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Tickets = new HashSet<Ticket>();
            this.ProjectUsers = new HashSet<ProjectUser>();
            this.TicketNotifications = new HashSet<TicketNotification>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketComments = new HashSet<TicketComments>();
            this.TicketAttachments = new HashSet<TicketAttachments>();
        }
        [ForeignKey("OwnerUserId")]
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; } 
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketComments> TicketComments { get; set; }
        public virtual ICollection<TicketAttachments> TicketAttachments { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketAttachments> TicketAttachments { get; set; }
        public virtual DbSet<TicketComments> TicketComments { get; set; }
        public virtual DbSet<TicketHistory> TicketHistories { get; set; }
        public virtual DbSet<TicketNotification> TicketNotifications { get; set; }
        public virtual DbSet<TicketStatus> TicketStatuses { get; set; }
        public virtual DbSet<TicketPriority> TicketPriorities { get; set; }
        public virtual DbSet<TicketType> TicketTypes { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectUser> ProjectUsers { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}