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
using System.Net.Mail;
using System.Net.Mime;
using StudentFinanceSupport.App_Start;

namespace StudentFinanceSupport.Controllers
{
    public class AdministratorsController : BaseController
    {

        
        public AdministratorsController()
        {
            //list of Actions in this controller that we don't want to security check session on
            bypassAdminCheck("ForgotPassword");
        }

        // GET: Administrators
        [AuthorizeUser(AccessLevel = "CreateAdmins")]
        public ActionResult Index()
        {
            //Administrator theAdmins = new Administrator();
            //theAdmins = db.Administrators();
            
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
                //Request.QueryString["[fromUrl]"] != "" && 
                if (Request.QueryString["fromUrl"] != null)
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

                    PasswordHashing thePassword = new PasswordHashing();
                    //string newPass = thePassword.Encrypt("123");

                    //check password match
                   
                    if (thePassword.passwordValid(password,user.Password))
                    {
                        IsValid = true;
                    }
                }
           
            return IsValid;
        }


        public void sendEmail()
        {
            //retrieve settings from webconfig
            string FromAddress = System.Configuration.ConfigurationManager.AppSettings.Get("FromAddress");
            string SmtpServer = System.Configuration.ConfigurationManager.AppSettings.Get("SmtpServer");
            string UserName = System.Configuration.ConfigurationManager.AppSettings.Get("UserName");
            string Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            string EnableSSL = System.Configuration.ConfigurationManager.AppSettings.Get("EnableSSL");
            string SMTPPort = System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort");
            string ReplyTo = System.Configuration.ConfigurationManager.AppSettings.Get("ReplyTo");

          

            var fromAddress = new MailAddress("no-reply@lukes-server.com", "no-reply");
            var toAddress = new MailAddress("mr.luke.hardiman@gmail.com");
            //const string fromPassword = Password;
            const string subject = "Subject";
            const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = SmtpServer,
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(UserName, Password)
               
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            
            {
                Subject = subject,
                Body = body,
                Priority = MailPriority.High,
                IsBodyHtml = true
                
            })
            {
                smtp.Send(message);
            }

        }

       

    }
}
