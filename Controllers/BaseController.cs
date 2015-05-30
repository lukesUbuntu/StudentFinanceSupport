using StudentFinanceSupport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StudentFinanceSupport.Controllers
{
    /// <summary>
    /// Purpose of my BaseController so we can overide in 1 place before returning back to the controller for admin checks
    /// </summary>
    public abstract class BaseController : Controller
    {
        public StudentRegistrationsModel db = new StudentRegistrationsModel();

        private bool debug = true;
        //Our predfined login route where to send non admin users needs to be set in route config
        private string LoginRoute = "Login";

        //set to allow a action set to by pass our checking
        private List<string> _actionsBypass = new List<string>();

      
        public BaseController()
        {
            //add the login route as we dont need to check for admin access here
            bypassAdminCheck(LoginRoute);
        }

        public void bypassAdminCheck(string theAction)
        {
            //add our own login to bypass our logged in check
            this._actionsBypass.Add(theAction);
        }
       

        private bool bypassAdminCheck(ActionExecutingContext filterContext)
        {
            //if the action is not in our list we will return -1
            int theCount = Array.FindIndex(_actionsBypass.ToArray(), x => x == filterContext.ActionDescriptor.ActionName.ToString());
            //filterContext.ActionDescriptor.ActionName.ToString().ToLower() == "login"
            if (UserLoggedIn() == true || theCount > -1)
            {
                return true;
            }
            return false;
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
            //return View();
        }
        //NoAcess   [GLOBAL] can be called from anywhere and will show user no access page
        public ActionResult NoAccess()
        {
            return View();
        }
        /// <summary>
        /// This is to overide OnActionExecuting https://msdn.microsoft.com/en-us/library/system.web.mvc.controller.onactionexecuting(v=vs.118).aspx
        /// it will be called for every action method in the class.
        /// </summary>
        /// <param name="filterContext">filterContext needs to be passed back to the base</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
                //need to make sure we check current action is not our login route otherwise we will cause a endless redirect
            if (bypassAdminCheck(filterContext) == false && debug == false)
              {
                 
                    //append the previous URL a user was trying to access so we can redirect them back
                  if (Request.RawUrl.Count() > 1)
                  {
                      Response.RedirectToRoute(LoginRoute, new { fromUrl = Request.Url.ToString() }); //defined it our routeConfig
                  }
                  else
                  {
                      Response.RedirectToRoute(LoginRoute); //defined it our routeConfig
                  }
                    
            }
            else
            {
                // you should be able to get session stuff here!!
                base.OnActionExecuting(filterContext);
            }
            
        }

        public AdministratorLogin AdminSession(){

            //return the AdministratorLogin from session
            if (Session["AdministratorLogin"] != null)
                return (AdministratorLogin)Session["AdministratorLogin"];

            return null;
        }

        
        /// <summary>
        /// Returns if the user has a session that is logged in
        /// </summary>
        /// <returns>true or false</returns>
        public bool UserLoggedIn()
        {
            return Helpers.Helpers.UserLoggedIn();
            //return (Session["logged_in"] != null) && (bool)Session["logged_in"] == true;
        }



        /// <summary>
        /// Checks if a student exists 
        /// </summary>
        /// <param name="theStudentID">ID Of student</param>
        /// <returns>true or false if exists</returns>
        public bool studentExists(string theStudentID)
        {
            //lets make sure we don't already have this Student_ID .Any(). will return a boolean if the entity was found
            return (db.StudentRegistrations.Any(m => m.Student_ID.ToLower() == theStudentID.ToLower()));
        }
    }


}