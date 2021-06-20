using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bug_Tracker.Models;
using Bug_Tracker.DAL;

namespace Bug_Tracker.BL
{
    public class TicketStatusService
    {
        private TicketStatusRepo repo;
        public TicketStatusService()
        {
            repo = new TicketStatusRepo();
        }
        public TicketStatusService(TicketStatusRepo repo)
        {
            this.repo = repo;
        }


        public void Create(TicketStatus ticketStatus)
        {
            repo.Add(ticketStatus);
        }
        public List<DashboardTicketChart> GetChartData(ApplicationUser user)
        {
            List<TicketStatus> ticketStatus = repo.GetCollection();

            List<DashboardTicketChart> chartData = new List<DashboardTicketChart>();

            if (user != null)
            {
                chartData = ticketStatus.Select(ts => new DashboardTicketChart
                {
                    Status = ts.Name,
                    StatusCount = ts.Tickets.Where(t => t.AssignedToUserId == user.Id || t.OwnerUserId == user.Id).Count(),
                }).ToList();
            }
            else
            {
                chartData = ticketStatus.Select(ts => new DashboardTicketChart
                {
                    Status = ts.Name,
                    StatusCount = ts.Tickets.Count,
                }).ToList();
            }

            return chartData;
        }
    }
}