﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models;
using StudentFinanceSupport.Models.functions;

namespace StudentFinanceSupport.Controllers
{
    public class RecoveryController : BaseController
    {
        // GET: Recovery
        public ActionResult Index()
        {
            
           
            //preset phone number digits
            ViewBag.phone_number = String.Empty;

            return View();
        }

        // GET: Recovery/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Recovery/Create
        public ActionResult Code()
        {
           
            return View();
        }

        // POST: Recovery/Create
        [HttpPost]
        public ActionResult Code(Recovery theRecovery)
        {
            var theAdmin = (from a in db.Administrators 
                            join b in db.Recoveries on a.UserId equals b.UserId
                            where b.recovery_key == theRecovery.recovery_key
                            select a).SingleOrDefault();

            if (theAdmin == null)
            {
                ModelState.AddModelError("recovery_key", "Incorrect Recovery KEY");
                return View(theRecovery);

            }
            //secure to only pass some details in session
           

            Session["AdministratorRecovery"] = (Recovery)theRecovery;

            return RedirectToAction("ChangePassword", "Administrators");
            //return View();
        }
        // GET: Recovery/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recovery/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Recovery/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Recovery/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Recovery/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Recovery/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public JsonResult adminDetails(Recovery theRecovery)
        {
            Administrator AdminAccount;

            //following state errors need to be cleared as we are not casting here
            ModelState["Administrator.FirstName"].Errors.Clear();
            ModelState["Administrator.Password"].Errors.Clear();
            ModelState["recovery_key"].Errors.Clear();

            //only check if model state is okay
            if (ModelState.IsValid)
            {
                AdminAccount = (from a in db.Administrators where a.Email == theRecovery.Administrator.Email select a).FirstOrDefault();
                if (AdminAccount == null)
                    ModelState.AddModelError("Administrator.Email", "Account does not exist.");
                else
                    theRecovery.Administrator = AdminAccount;
            }
                
            //check the rest of the model state
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(
                     kvp => kvp.Key,
                     kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                 );

                return Json(errorList, JsonRequestBehavior.AllowGet);
            }

            string mobile = theRecovery.Administrator.mobile.ToString();

            /*
            
            RecoveryComms theComms = new RecoveryComms();
            if (theComms.sendSMS(ref theRecovery) == true)
            {
                db.Recoveries.Add(theRecovery);
                db.SaveChanges();
            }
            mobile = mobile.Remove(mobile.Length - 4, 4) + "****";
            */
            var theResponse = Json(new     
                {     
                    success = "true",     
                    id = "s",
                    mobile_guess = mobile     
                });
          

          

            //we end here with a correct recovery
           
            //no errors lets check 
            //str = str.Remove(str.Length - 3);
            //string response = theRecovery.Administrator.Email;

            return Json(theResponse.Data, JsonRequestBehavior.AllowGet);
        }
    }
}
