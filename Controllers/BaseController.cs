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
        //Our predfined login route where to send non admin users needs to be set in route config
        private string LoginRoute = "Login";

        //set to allow a action set to by pass our checking
        private List<string> _actionsBypass = new List<string>();

      
        public BaseController()
        {
            //add the login route we redirect to
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
        /// <summary>
        /// This is to overide OnActionExecuting https://msdn.microsoft.com/en-us/library/system.web.mvc.controller.onactionexecuting(v=vs.118).aspx
        /// it will be called for every action method in the class.
        /// </summary>
        /// <param name="filterContext">filterContext needs to be passed back to the base</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            
                //need to make sure we check current action is not our login route otherwise we will cause a endless redirect
               if (bypassAdminCheck(filterContext) == false)
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

        /// <summary>
        /// Returns if the user has a session that is logged in
        /// </summary>
        /// <returns>true or false</returns>
        public bool UserLoggedIn()
        {
            return Helpers.Helpers.UserLoggedIn();
            //return (Session["logged_in"] != null) && (bool)Session["logged_in"] == true;
        }

        private string getAC()
        {

           
            // Split the url to url + query string
            if (Request.UrlReferrer == null) return null;

            var fullUrl = Request.UrlReferrer.ToString();
            var questionMarkIndex = fullUrl.IndexOf('?');
            string queryString = null;
            string url = fullUrl;
            if (questionMarkIndex != -1) // There is a QueryString
            {    
                url = fullUrl.Substring(0, questionMarkIndex); 
                queryString = fullUrl.Substring(questionMarkIndex + 1);
            }


            return queryString;
           
        }

    }


}