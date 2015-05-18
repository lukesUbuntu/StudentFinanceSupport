using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models;
using System.Web.Security;

namespace StudentFinanceSupport.Controllers
{
    public class AdministratorsController : BaseController
    {
        private AdministratorsModel db = new AdministratorsModel();

        public AdministratorsController()
        {
            //list of Actions in this controller that we don't want to security check session on
            bypassAdminCheck("ForgotPassword");
        }

        // GET: Administrators
        public ActionResult Index()
        {
            return View(db.Administrators.ToList());
        }

        

        // GET: Administrators/ForgotPassword
        public ActionResult ForgotPassword()
        {
            //this.bypassAdminCheck();
            return View();
        }

       
  
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
       }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
            //return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            //this.bypassAdminCheck();
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.AdministratorLogin user)
        {

            if (IsValid(user.Email, user.Password))
            
            {
                //FormsAuthenticationTicket with the supplied username & persistence options, serializes it,
                FormsAuthentication.SetAuthCookie(user.Email, false);
                //http://stackoverflow.com/questions/23301445/formsauthentication-setauthcookie-vs-formsauthentication-encrypt
                Session["logged_in"] = true;
                //return RedirectToAction("Index", "Home");
                //Request.QueryString["fromUrl"]

                if (Request.QueryString["[fromUrl]"] != "" && Request.QueryString["[fromUrl]"] != null)
                {

                    return Redirect(Request.QueryString["fromUrl"]);
                }  
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login details are wrong.");
            }

            return View(user);
        }

        
        /// <summary>
        ///  IsValid checks a email and password against the database
        ///  @todo encrypt or salt password
        /// </summary>
        /// <param name="email">email as username</param>
        /// <param name="password">password</param>
        /// <returns>returns true or false</returns>
        private bool IsValid(string email, string password)
        {
            
            bool IsValid = false;

            //grab the user
            var user = db.Administrators.FirstOrDefault(theUser => theUser.Email == email);
                if (user != null)
                {
                    //check password match
                    if (user.Password == password)
                    {
                        IsValid = true;
                    }
                }
           
            return IsValid;
        }

       

    }
}
