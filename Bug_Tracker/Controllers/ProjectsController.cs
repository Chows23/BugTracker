using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.BL;

namespace Bug_Tracker.Controllers
{
    public class ProjectsController : Controller
    {
        public ProjectService projectService { get; set; }
        public ActionResult Index()
        {

            return View();
        }
    }
}