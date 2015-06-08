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
            bypassAdminCheck("Administrators/Login");
            bypassAdminCheck("Administrators/ChangePassword");
            
            //bypassAdminCheck("Administrators/Login");
        }

        // GET: Administrators
       // [AuthorizeUser(AccessLevel = "CreateAdmins")]
        public ActionResult Index()
        {
            //Administrator theAdmins = new Administrator();
            //theAdmins = db.Administrators();
            
            return View();
        }

        // GET: Administrators/Add
        [Authorize(Roles = "Admin")]
        public ActionResult Admins()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            
            return View(db.Administrators.ToList());
        }
       // [HttpPost]
        public ActionResult ChangePassword()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
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

            CurrentAdmin.Password = "";
             return View(CurrentAdmin);
        }

        [HttpPost]
        public ActionResult ChangePassword(AdministratorLogin theAdmin)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            //passing back from session so no injection of userID or email can happen we also need to clear the model state and re-validate
            ModelState.Clear();
            theAdmin.Email = this.AdminSession().Email;
            theAdmin.UserId = this.AdminSession().UserId;
            TryValidateModel(theAdmin);  

            //ModelState.Clear();
            //check password match
            if (theAdmin.Password != Request.Form["password_match"])
            {
                //clear the viewbag password so they re-type
                ViewBag.password_match = String.Empty;
                ModelState.AddModelError("Password", "Passwords don't match");
            }
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        Console.Write(error);
                    }
                }
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

        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            Administrator theAdmins = new Administrator();

            //theAdmins = db.Administrators();



            //var role_list = db.RoleTypes;
            //var model = new Administrator();
            //model.Roles = new SelectList(db.RoleTypes, "role_type_id", "role_name");
            ViewBag.role_type_id = new SelectList(db.RoleTypes, "role_type_id", "role_description");
            ViewBag.password_match = String.Empty;
            return View(theAdmins);
        }

        [HttpPost]
        public ActionResult Add(Administrator newAdmin)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            //newAdmin.Roles = Request.Form["admin_roles"];

            ViewBag.role_type_id = new SelectList(db.RoleTypes, "role_type_id", "role_description");
            //ViewBag.Roles = Request.Form["admin_roles"];
            ViewBag.password_match = Request.Form["password_match"];
            
           
            //check for unique email address
            //if (ModelState.ContainsKey("Roles"))
              //  ModelState["Roles"].Errors.Clear();
            
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
            //this.addRoles(newAdmin, Request.Form["roles"]);

            
            return RedirectToAction("Admins");
        }
         [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }
        // GET: Administrators1/Edit/5
         [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Administrator administrator = db.Administrators.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_type_id = new SelectList(db.RoleTypes, "role_type_id", "role_name", administrator.role_type_id);
            return View(administrator);
        }

        // POST: Administrators1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,Email,Password,FirstName,LastName,mobile,role_type_id")] Administrator administrator)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            if (ModelState.IsValid)
            {
                db.Entry(administrator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Admins");
            }
            ViewBag.role_type_id = new SelectList(db.RoleTypes, "role_type_id", "role_name", administrator.role_type_id);
            return View(administrator);
        }


        // POST: Administrators1/Delete/5
         [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            Administrator administrator = db.Administrators.Find(id);
            db.Administrators.Remove(administrator);
            db.SaveChanges();
            return RedirectToAction("Admins");
        }
      
      

       
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdministratorLogin administrator)
        {

            if (IsValid(ref administrator))
            
            {

               
                
                
                
                //FormsAuthenticationTicket with the supplied username & persistence options, serializes it,
                
                
                
                FormsAuthentication.SetAuthCookie(administrator.Email, false);
                //Roles.AddUserToRole(administrator.Email, "Admin");
                //http://stackoverflow.com/questions/23301445/formsauthentication-setauthcookie-vs-formsauthentication-encrypt
                Session["logged_in"] = true;
                Session["AdministratorLogin"] = administrator;
                //set users roles


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

            return View(administrator);
        }

        
        /// <summary>
        ///  IsValid checks a email and password against the database
        ///  @todo encrypt or salt password
        /// </summary>
        /// <param name="AdministratorLogin">by ref pass AdministratorLogin details</param>
        /// <returns>returns true or false</returns>
        private bool IsValid(ref AdministratorLogin administrator)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            bool IsValid = false;
            string admin_email = administrator.Email;
            //grab the user
            var user = db.Administrators.FirstOrDefault(theUser => theUser.Email == admin_email);

                if (user != null)
                {
                    if (PasswordHashing.passwordValid(administrator.Password, user.Password))
                    {
                        //account is valid we need to update our admin account with the ID
                        administrator.UserId = user.UserId;
                        IsValid = true;
                       
                    }
                }
           
            return IsValid;
        }



       

       

    }
}
