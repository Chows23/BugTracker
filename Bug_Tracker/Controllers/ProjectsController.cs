﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.BL;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Bug_Tracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectService projectService = new ProjectService();
        private ProjectUserService projectUserService = new ProjectUserService();
        private UserService userService = new UserService();
        public static ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = userManager.FindById(User.Identity.GetUserId());
            else
                return new HttpUnauthorizedResult();
            
            return View(user.ProjectUsers.Select(p => p.Project));
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult AllProjects()
        {
            return View(projectUserService.GroupedByProject());
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Create([Bind(Include = "Name")] Project project)
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = userManager.FindById(User.Identity.GetUserId());
            else
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {
                projectService.Create(project);
                ProjectUser newProjectUser = new ProjectUser
                {
                    ProjectId = project.Id,
                    UserId = user.Id
                };
                projectUserService.Create(newProjectUser);
                return RedirectToAction("Edit", "Projects", new { id = project.Id });
            }

            return View(project);
        }
    }
}