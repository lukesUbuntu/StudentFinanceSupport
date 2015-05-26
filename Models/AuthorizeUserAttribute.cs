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

            string privilegeLevels = string.Join("", GetUserRoles(httpContext.User.Identity.Name.ToString())); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.AccessLevel))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetUserRoles(string username)
        {
            if (username == null) return null;
            //get users roles
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            var theAdmin = db.Administrators.FirstOrDefault(theUser => theUser.Email == username);

            return theAdmin.UserType;
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
