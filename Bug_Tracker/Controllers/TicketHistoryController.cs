﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class TicketHistoryController : Controller
    {
        // GET: TicketHistory
        public ActionResult Index()
        {
            return View();
        }
    }
}