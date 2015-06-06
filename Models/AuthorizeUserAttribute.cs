using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StudentFinanceSupport.Models
{
    //https://msdn.microsoft.com/en-us/liBrary/8fw7xh74(v=vs.100).aspx
    //https://msdn.microsoft.com/en-us/library/317sza4k(v=vs.140).aspx
    //http://stackoverflow.com/questions/25435763/asp-net-mvc-custom-role-provider-not-working
    public class CustomRoleProvider : RoleProvider
    {

        public override string ApplicationName { get; set; }

   
        public override void AddUsersToRoles(string[] usernames, string[] roleNames) {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName) {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();

        }
   
      
 
      
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        public override string[] GetRolesForUser(string username)
        {
           
            using (var db = new StudentRegistrationsModel())
            {
                var user = db.Administrators.FirstOrDefault(u => u.Email.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                  var adminRoles = from theAdmin in db.Administrators
                              where theAdmin.Email == username
                              join theRoles in db.Roles on theAdmin.UserId equals theRoles.UserId
                              join theAdminRoles in db.RoleTypes on theRoles.role_type_id equals theAdminRoles.role_type_id
                              select theAdminRoles.role_name;

                return adminRoles.ToArray();

                 
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = new StudentRegistrationsModel())
            {
              
                  var adminRoles = from theAdmin in db.Administrators
                              where theAdmin.Email == username
                              join theRoles in db.Roles on theAdmin.UserId equals theRoles.UserId
                              join theAdminRoles in db.RoleTypes on theRoles.role_type_id equals theAdminRoles.role_type_id
                              where theAdminRoles.role_name == roleName select theAdmin;
                             

                return (adminRoles != null) ? true : false;
            }
        }
    
    }
  
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        // Custom property
        public string AccessLevel { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            return GetUserRoles(httpContext.User.Identity.Name.ToString(), this.AccessLevel); // Call another method to get rights of the user from DB

        }

        public bool GetUserRoles(string username, string access_level)
        {
            if (username == null || access_level == null) return false;
            //get users roles
            try {

                StudentRegistrationsModel db = new StudentRegistrationsModel();

                var adminRoles = from theAdmin in db.Administrators
                              where theAdmin.Email == username
                              join theRoles in db.Roles on theAdmin.UserId equals theRoles.UserId
                              join theAdminRoles in db.RoleTypes on theRoles.role_type_id equals theAdminRoles.role_type_id
                              select new 
                              {
                                  role_name = theAdminRoles.role_name

                              };


                foreach (var role in adminRoles){
                    if (role.role_name == access_level) return true;
                }
                return false;
            }
            catch (Exception e) {

                return false;
            }
            
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Controller.TempData["ErrorDetails"] = "You must be logged in to access this page";
                filterContext.Result = new RedirectResult("~/Administrators/Login");
                return;
            }
            if (filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Controller.TempData["ErrorDetails"] = "You don't have access rights to this page";
                filterContext.Result = new RedirectResult("~/Administrators/NoAccess");
                return;
            }
        }
    }
}
