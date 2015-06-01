using System;
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
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            var theAdmin = (from a in db.Administrators
                            join b in db.Recoveries on a.UserId equals b.UserId
                            where b.recovery_key == theRecovery.recovery_key
                            select a).SingleOrDefault();

            if (theAdmin == null)
            {
                ModelState.AddModelError("recovery_key", "Incorrect Recovery KEY Details");
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

        private void clearErrorStates(List<string> theKeys)
        {
            foreach (var key in theKeys)
            {
                if (ModelState.ContainsKey(key))
                    ModelState[key].Errors.Clear();
            }

        }
        public JsonResult adminDetails(Recovery theRecovery)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            Administrator AdminAccount;
            List<string> skipKeys = new List<string>();
            skipKeys.Add("Administrator.FirstName");
            skipKeys.Add("Administrator.Password");
            skipKeys.Add("recovery_key");
            clearErrorStates(skipKeys);
       
            //only check if model state is okay
            if (ModelState.IsValid)
            {
                AdminAccount = (from a in db.Administrators where a.Email == theRecovery.Administrator.Email select a).FirstOrDefault();
                if (AdminAccount == null)
                    ModelState.AddModelError("Administrator.Email", "Account does not exist.");
                else
                    theRecovery.Administrator = AdminAccount;
 
            }
            theRecovery.UserId = theRecovery.Administrator.UserId;

            //check the rest of the model state
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(
                     kvp => kvp.Key,
                     kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                 );

                return Json(errorList, JsonRequestBehavior.AllowGet);
            }

            Session["AdministratorRecovery"] = theRecovery;

            if (theRecovery.recovery_option == "mobile")
            {
              
                string mobile = theRecovery.Administrator.mobile.Remove(theRecovery.Administrator.mobile.Length - 4, 4) + "****";
                return Json(new
                {
                    success = "true",
                    mobile_guess = mobile
                }, JsonRequestBehavior.AllowGet);

            }



           

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult sendRecoveryCode(Recovery theRecovery)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();

                //session has been removed
                if (Session["AdministratorRecovery"] == null)
                {
                    return Json(new
                    {
                        success = "false",
                        message = "Invalid State"
                    }, JsonRequestBehavior.AllowGet);
                }


                Recovery sessRecovery = (Recovery)Session["AdministratorRecovery"];
                theRecovery.Administrator.FirstName = sessRecovery.Administrator.FirstName;
                theRecovery.Administrator.Password = sessRecovery.Administrator.Password;
                theRecovery.UserId = sessRecovery.Administrator.UserId;
                //ModelState.Clear();

                if (theRecovery.recovery_option == "email")
                {
                    RecoveryComms theComms = new RecoveryComms();
                    if (theComms.sendEmail(ref theRecovery) == true)
                    {


                       

                        return Json(new
                        {
                            success = "true",
                            message = "Sent email request"

                        }, JsonRequestBehavior.AllowGet);
                       
                    }
                }

                
                //requesting for recovery code
                if (theRecovery.recovery_option == "mobile")
                {
               
                    string attemptGuess = theRecovery.Administrator.mobile;
                    //int.TryParse(theRecovery.Administrator.mobile,out guessNumber);



                    string correctAttempt = new String(sessRecovery.Administrator.mobile.
                                Where(x => Char.IsDigit(x)).Reverse().Take(4).Reverse().ToArray());

                    //success here if correct number
                    if (theRecovery.Administrator.mobile == correctAttempt)
                    {
                        //passover the correct mobile details
                        //theRecovery.Administrator.mobile = sessRecovery.Administrator.mobile;
                        //theRecovery.UserId = sessRecovery.Administrator.UserId;

                        RecoveryComms theComms = new RecoveryComms();
                        if (theComms.sendSMS(ref sessRecovery) == true)
                        {
                            db.Recoveries.Add(sessRecovery);
                            db.SaveChanges();
                            return Json(new
                            {
                                success = "true",
                                message = "sent SMS to mobile"

                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            success = "false",
                            message = "Failed incorrect last digits"

                        }, JsonRequestBehavior.AllowGet);
                    }
            }
           
                    
            
           
            return Json("", JsonRequestBehavior.AllowGet);
        }


       

    }
}
