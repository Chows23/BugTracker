using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Bug_Tracker.BL;
using Bug_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace Bug_Tracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ProjectService projectService = new ProjectService();
        private ProjectUserService projectUserService = new ProjectUserService();
        private TicketService ticketService = new TicketService();

        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = UserService.GetUser(User.Identity.Name);
            else
                return new HttpUnauthorizedResult();

            if (UserService.UserInRole(user.Id, "admin"))
                return RedirectToAction("AllProjects");

            ViewBag.Notifications = user.TicketNotifications.Count;
            var projects = projectUserService.GetUsersProjects(user.Id).Select(pu => pu.Project).ToList();
            return View(projects);
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult AllProjects()
        {
            return View(projectService.AllProjects().ToList());
        }

        [Authorize(Roles = "admin, manager")]
        public ActionResult Create()
        {
            ViewBag.ManagerId = new SelectList(UserService.GetUserByRole("manager"), "Id", "UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Create([Bind(Include = "Name")] Project project, string managerId)
        {
            ApplicationUser user;
            if (User.Identity.IsAuthenticated)
                user = UserService.GetUser(User.Identity.Name);
            else
                return new HttpUnauthorizedResult();

            if (ModelState.IsValid)
            {

                if (UserService.UserInRole(user.Id, "admin") && managerId != null)
                {
                    var newProjectUser = projectUserService.ProjectUser(managerId, project.Id);
                    newProjectUser.Project = project;
                    var manager = UserService.GetUserById(managerId);
                    manager.ProjectUsers.Add(newProjectUser);
                    projectUserService.Create(newProjectUser);
                }
                else
                {
                    var newProjectUser = projectUserService.ProjectUser(user.Id, project.Id);
                    newProjectUser.Project = project;
                    user.ProjectUsers.Add(newProjectUser);
                    projectUserService.Create(newProjectUser);
                }

                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            ViewBag.ManagerId = new SelectList(UserService.GetUserByRole("manager"), "Id", "UserName");

            return View(project);
        }

        [Authorize]
        public ActionResult Details(int? id, int? page, int? pageSize)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var project = projectService.GetProject((int)id);

            if (project == null)
                return HttpNotFound();

            var user = UserService.GetUser(User.Identity.Name);
            var projectUser = projectUserService.GetExistingProjectUser(project.Id, user.Id);
            ProjectDetailsViewModel projectDetailsViewModel = new ProjectDetailsViewModel();

            if (projectUser == null && !UserService.UserInRole(user.Id, "admin"))
            {
                if (UserService.UserInRole(user.Id, "submitter") || UserService.UserInRole(user.Id, "developer"))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                else if (UserService.UserInRole(user.Id, "manager"))
                {
                    ViewBag.TicketCount = project.Tickets.Count;
                    projectDetailsViewModel = projectService.ProjectDetailsViewModel(project.Id, project.Name, project.ProjectUsers.ToList(), null);
                }
            }
            else
            {               
                if (pageSize == null)
                    pageSize = 3;

                ViewBag.PageSize = pageSize;
                int pageNumber = (page ?? 1);

                var tickets = projectService.GetUserTicketsOnProject(user.Id, project.Tickets.ToList());
                projectDetailsViewModel = projectService.ProjectDetailsViewModel(project.Id, project.Name, project.ProjectUsers.ToList(), tickets.ToPagedList(pageNumber, (int)pageSize));
            }
            ViewBag.AddUserId = new SelectList(UserService.GetAddToProjectUsers(project.Id), "Id", "UserName");
            ViewBag.RemoveUserId = new SelectList(UserService.GetRemoveFromProjectUsers(project.Id), "Id", "UserName");

            return View(projectDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult EditName([Bind(Include = "Id,Name")] Project project)
        {
            if (ModelState.IsValid)
            {
                projectService.Update(project);
                return RedirectToAction("Details", new { id = project.Id });
            }
            else
                TempData["Error"] = "Your project is missing something";

            return RedirectToAction("Details", new { id = project.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult AddUser(int? id, string addUserId)
        {
            
            if (id == null || addUserId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = UserService.GetUserById(addUserId);
            var project = projectService.GetProject((int)id);

            if (user == null || project == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                if (!projectUserService.CheckIfUserOnProject((int)id, addUserId))
                {
                    var newProjectUser = projectUserService.ProjectUser(addUserId, project.Id);
                    projectUserService.Create(newProjectUser);
                }
            }

            return RedirectToAction("Details", new { id = project.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult RemoveUser(int? id, string removeUserId)
        {
            if (id == null || removeUserId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = UserService.GetUserById(removeUserId);
            var project = projectService.GetProject((int)id);

            if (user == null || project == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                if (projectUserService.CheckIfUserOnProject((int)id, removeUserId))
                {
                    var projectUserToRemove = projectUserService.GetExistingProjectUser((int)id, removeUserId);
                    projectUserService.RemoveProjectUser(projectUserToRemove);
                    ticketService.UnassignUserTickets(project.Id, user.Id);
                }
            }

            return RedirectToAction("Details", new { id = project.Id });
        }
    }
}