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
        public RecoveryController()
        {
            //bypass the complete controller no need for session checking
            bypassControllerCheck("Recovery");
        }
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

       

        // POST: Recovery/Create
        /// <summary>
        ///  Checks and verifyies a recovery key code 
        /// </summary>
        /// <param name="recovery_key">JSON response</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Code(String recovery_key)
        {
            if (String.IsNullOrEmpty(recovery_key))
            {
                return Json(new
                {
                    success = false,
                    message = "Please eter a recovery key"
                }, JsonRequestBehavior.AllowGet);
            }

            StudentRegistrationsModel db = new StudentRegistrationsModel();
            /*var theAdmin = (from a in db.Administrators
                            join b in db.Recoveries on a.UserId equals b.UserId
                            where b.recovery_key == theRecoverCodey
                            select a).SingleOrDefault();*/
            var theAdmin = (from a in db.Recoveries
                            where a.recovery_key == recovery_key
                            join b in db.Administrators on a.UserId equals b.UserId
                            //where b.recovery_key == theRecoverCodey
                            select a).SingleOrDefault();

            if (theAdmin == null)
            {
              
                return Json(new
                {
                    success = false,
                    message = "Incorrect Recovery KEY Details"
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //set session the sesion
                Session["AdministratorRecovery"] = (Recovery)theAdmin;
                return Json(new
                {
                    success = true,
                    message = "<b>Success</b> Redirecting you now...",
                    url = "/Administrators/ChangePassword"

                }, JsonRequestBehavior.AllowGet);
            }
            //secure to only pass some details in session


           

            //return RedirectToAction("ChangePassword", "Administrators");
            //return View();
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
            //dispose as no longer needed
            db.Dispose();
            theRecovery.UserId = theRecovery.Administrator.UserId;

            //check the rest of the model state and send back the errors to client
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(
                     kvp => kvp.Key,
                     kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                 );

                return Json(new
                {
                    success = false,
                    errors = errorList
                }, JsonRequestBehavior.AllowGet);
                
            }

            Session["AdministratorRecovery"] = theRecovery;

            if (theRecovery.recovery_option == "mobile")
            {
              
                string mobile = theRecovery.Administrator.mobile.Remove(theRecovery.Administrator.mobile.Length - 4, 4) + "****";
                return Json(new
                {
                    success = true,
                    mobile_guess = mobile
                }, JsonRequestBehavior.AllowGet);

            }


            return Json(new
            {
                success = true,
                message = "You can now request a reset a recovery code.."
            }, JsonRequestBehavior.AllowGet);


            //return sendRecoveryCode(theRecovery);

           
        }

        /// <summary>
        ///  Sends the recovery code to the user via email or sms
        /// </summary>
        /// <param name="theRecovery">Recovery details</param>
        /// <returns>JSON if has passed Recovery checks</returns>
        public JsonResult sendRecoveryCode(Recovery theRecovery)
        {
            

                //session has been removed
                if (Session["AdministratorRecovery"] == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Invalid Session please refresh the browser"
                    }, JsonRequestBehavior.AllowGet);
                }

                StudentRegistrationsModel db = new StudentRegistrationsModel();

                Recovery sessRecovery = (Recovery)Session["AdministratorRecovery"];

                theRecovery.Administrator.FirstName = sessRecovery.Administrator.FirstName;
                theRecovery.Administrator.Password = sessRecovery.Administrator.Password;
                theRecovery.UserId = sessRecovery.Administrator.UserId;
                //ModelState.Clear();

                if (theRecovery.recovery_option == "email")
                {
                    RecoveryComms theComms = new RecoveryComms();
                    if (theComms.sendEmail(ref sessRecovery) == true)
                    {


                        //db.Recoveries.Attach(theRecovery);
                        db.Recoveries.Add(sessRecovery);
                        db.SaveChanges();

                        return Json(new
                        {
                            success = true,
                            message = String.Format("<b>Great!</b>, we have sent your recovery code to <b>{0}</b>", sessRecovery.Administrator.Email)

                        }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Something bad happend"

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
                            //db.Recoveries.Attach(theRecovery);
                            db.Recoveries.Add(sessRecovery);
                            db.SaveChanges();
                           
                            return Json(new
                            {
                                success = true,
                                message =  String.Format("Great we have sent you a TEXT Message with recovery code to <b>{0}</b>", sessRecovery.Administrator.mobile)

                            }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {

                            return Json(new
                            {
                                success = false,
                                message = "Something bad happend"

                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Failed incorrect last digits"

                        }, JsonRequestBehavior.AllowGet);
                    }
            }
           
                    
            
           
            return Json("", JsonRequestBehavior.AllowGet);
        }


       

    }
}
