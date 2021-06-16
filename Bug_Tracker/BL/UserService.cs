using Bug_Tracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace Bug_Tracker.BL
{
    public class UserService
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        private static RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(db)
        );

        private static UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(db)
        );

        private static ProjectUserService projectUserService = new ProjectUserService();

        public static IEnumerable<string> GetAllRoles()
        {
            return roleManager.Roles.Select(r => r.Name).ToList();
        }

        public static bool CreateUser(ApplicationUser user, string password)
        {
            var result = userManager.Create(user, password);
            return result.Succeeded;
        }

        // Get User
        public static ApplicationUser GetUser(string username)
        {
            return db.Users.FirstOrDefault(u => u.UserName == username);
        }

        //Get User by Id
        public static ApplicationUser GetUserById(string id)
        {
            return db.Users.FirstOrDefault(u => u.Id == id);
        }

        //Check if user is in a role
        public static bool UserInRole(string userId, string role)
        {
            return userManager.IsInRole(userId, role);
        }

        // Add User To Role
        public static bool AddUserToRole(string userId, string role)
        {
            if (UserInRole(userId, role))
                return false;

            userManager.AddToRole(userId, role);
            return true;
        }

        // Remove User From Role
        public static bool RemoveUserFromRole(string userId, string role)
        {
            if (!UserInRole(userId, role))
                return false;

            userManager.RemoveFromRole(userId, role);
            return true;
        }

        // AddRole
        public static void AddRole(string role)
        {
            if (!roleManager.RoleExists(role))
                roleManager.Create(new IdentityRole { Name = role });
        }

        // RemoveRole
        public static void RemoveRole(string role)
        {
            if (roleManager.RoleExists(role))
                roleManager.Delete(roleManager.FindByName(role));
        }

        // Get all roles of user
        public static IEnumerable<string> GetAllRolesOfUser(string userId)
        {
            return userManager.GetRoles(userId);
        }

        // Get possible users to add to a project
        public static List<ApplicationUser> GetAddToProjectUsers(int projectId)
        {
            List<ApplicationUser> result = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if (!projectUserService.CheckIfUserOnProject(projectId, user.Id) && (UserInRole(user.Id, "developer") || UserInRole(user.Id, "manager") || UserInRole(user.Id, "submitter")))
                {
                    result.Add(user);
                }
            }

            return result;
        }

        // Get possible users to remove from a project
        public static List<ApplicationUser> GetRemoveFromProjectUsers(int projectId)
        {
            List<ApplicationUser> result = new List<ApplicationUser>();
            foreach (var user in db.Users.ToList())
            {
                if (projectUserService.CheckIfUserOnProject(projectId, user.Id) && (UserInRole(user.Id, "developer") || UserInRole(user.Id, "manager") || UserInRole(user.Id, "submitter")))
                {
                    result.Add(user);
                }
            }

            return result;
        }

        // Get all managers
        public static List<ApplicationUser> AllManagers()
        {
            return db.Users.ToList().Where(u => UserInRole(u.Id, "manager")).ToList();
        }
    }
}