using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models;
using StudentFinanceSupport.Models.functions;
using System.Web.Security;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;

namespace StudentFinanceSupport.Controllers
{
    public class AdministratorsController : BaseController
    {

        
        public AdministratorsController()
        {
            //list of Actions in this controller that we don't want to security check session on
            //bypassAdminCheck("ForgotPassword");
        }

        // GET: Administrators
       // [AuthorizeUser(AccessLevel = "CreateAdmins")]
        public ActionResult Index()
        {
            //Administrator theAdmins = new Administrator();
            //theAdmins = db.Administrators();
            
            return View(db.Administrators.ToList());
        }

        // GET: Administrators/Add
      
       
       // [HttpPost]
        public ActionResult ChangePassword()
        {
            //@important don't pass the administor model for modifying ONLY the AdministratorLogin model for security as this is public

            //check if we are coming from a recovery
            if (Session["AdministratorRecovery"] != null)
            {
                //remove any logged in session
                Session.Remove("AdministratorLogin");
                
                Recovery theRecovery = (Recovery)Session["AdministratorRecovery"];
                var theAdmin = (from a in db.Administrators
                                join b in db.Recoveries on a.UserId equals b.UserId
                                where b.recovery_key == theRecovery.recovery_key
                                select a).SingleOrDefault();
                //clear the password field
                theAdmin.Password = String.Empty;
                Session["AdministratorLogin"] = theAdmin.loginDetails();
                return View(theAdmin.loginDetails());
            }

            AdministratorLogin CurrentAdmin = this.AdminSession();
            //check if we are logged in
            if (CurrentAdmin == null)
            {

                return RedirectToAction("Login");
            }

             
             return View(CurrentAdmin);
        }

        [HttpPost]
        public ActionResult ChangePassword(AdministratorLogin theAdmin)
        {
            ModelState.Clear();
            //check password match
            if (theAdmin.Password != Request.Form["password_match"])
            {
                //clear the viewbag password so they re-type
                ViewBag.password_match = String.Empty;
                ModelState.AddModelError("Password", "Passwords don't match");
            }
            if (!ModelState.IsValid)
            {
                return View(theAdmin);
            }

            //grab the current admin session and update password
            //process the update
            AdministratorLogin thisUser = this.AdminSession();

            var change = (from a in db.Administrators
                          where a.UserId == thisUser.UserId
                            select a).SingleOrDefault();

            change.Password = PasswordHashing.Encrypt(theAdmin.Password);

            //clean up from recovery
            if (Session["AdministratorRecovery"] != null)
            {
                Session.Remove("AdministratorRecovery");
                //remove any recovery options that are set
                var recovery = (from b in db.Recoveries where b.UserId == change.UserId select b);
                foreach (var entry in recovery)
                    db.Recoveries.Remove(entry);
            }
               

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Add()
        {
            Administrator theAdmins = new Administrator();

            //theAdmins = db.Administrators();



            var role_list = db.RoleTypes;
            //var model = new Administrator();
            //model.Roles = new SelectList(db.RoleTypes, "role_type_id", "role_name");
            ViewBag.admin_roles = new MultiSelectList(db.RoleTypes, "role_type_id", "role_description");
            ViewBag.password_match = String.Empty;
            return View(theAdmins);
        }

        [HttpPost]
        public ActionResult Add(Administrator newAdmin)
        {
            //newAdmin.Roles = Request.Form["admin_roles"];
           
            ViewBag.admin_roles = new MultiSelectList(db.RoleTypes, "role_type_id", "role_description");
            ViewBag.Roles = Request.Form["admin_roles"];
            ViewBag.password_match = Request.Form["password_match"];
            
           
            //check for unique email address
            if (ModelState.ContainsKey("Roles"))
                ModelState["Roles"].Errors.Clear();

            if (db.Administrators.Any(m => m.Email.ToLower() == newAdmin.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Admin email already exists!");
            
            }
            //check password match
            if (newAdmin.Password != Request.Form["password_match"])
            {
                //clear the viewbag password so they re-type
                ViewBag.password_match = String.Empty;
                ModelState.AddModelError("Password", "Passwords don't match");
            }

            //check mobile phone number if added
            if (!String.IsNullOrEmpty(newAdmin.mobile)) { 
                Regex mobileRegex = new Regex(@"^(?:\(?)(?:\+|0{2})([0-9]{3})\)? ([0-9]{2}) ([0-9]{7})$");
                if (!mobileRegex.IsMatch(newAdmin.mobile))
                {
                    ModelState.AddModelError("mobile", "Incorect mobile format example : 02107770777 or (021)07770777");
                }
            }
            if (!ModelState.IsValid)
            {
                return View(newAdmin);
            }

            //PasswordHashing passwordHash = new PasswordHashing();
            newAdmin.Password = PasswordHashing.Encrypt(newAdmin.Password);

            //add the admin
            db.Administrators.Add(newAdmin);
            db.SaveChanges();

            //we will now have admin id if saved
            //int theAdmin_id = newAdmin.UserId;
            //check roles that need to be added

            //store roles into a string array
            this.addRoles(newAdmin, Request.Form["roles"]);

            
            return RedirectToAction("Index");
        }

        

        /// <summary>
        /// Updates or adds roles to an admin
        /// </summary>
        /// <param name="theAdmin">Administrator</param>
        /// <param name="theRoles">Roles_ID from roletypes as comma delimited</param>
        private void addRoles(Administrator theAdmin,String theRoles)
        {
            string[] AddRoles = theRoles.Split(new Char[] { ',' });
            //incase we are updating lets remove previous roles
            //lets check previous roles and remove them
            var old_roles = from s in db.Roles
                            where s.UserId == theAdmin.UserId
                            select s.role_id;
            //remove
            if (old_roles.Count() > 0)
            foreach (var role_type in old_roles)
            {
                int role_id = Convert.ToInt32(role_type);
                Role rmvRole = db.Roles.Find(role_id);
                db.Roles.Remove(rmvRole);
            }           

            //add in new roles
            if (AddRoles.Count() > 0)
            {
                    


                var roles = new Role();
                roles.UserId = theAdmin.UserId;
                foreach (var role_type in AddRoles)
                {
                    int role_id = Convert.ToInt32(role_type);
                    roles.role_type_id = role_id;
                    db.Roles.Add(roles);
                }
                db.SaveChanges();
            }
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
                Session["AdministratorLogin"] = user;

                //return RedirectToAction("Index", "Home");

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

                   // PasswordHashing thePassword = new PasswordHashing();
                    //string newPass = thePassword.Encrypt("123");

                    //check password match

                    if (PasswordHashing.passwordValid(password, user.Password))
                    {
                        IsValid = true;
                    }
                }
           
            return IsValid;
        }



        public void sendEmail()
        {
           

        }

       

    }
}
