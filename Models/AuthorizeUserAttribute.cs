using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace StudentFinanceSupport.Models
{
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
