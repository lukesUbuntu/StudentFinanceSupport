using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models;
using StudentFinanceSupport.Helpers;

namespace StudentFinanceSupport.Controllers
{
    public class StudentVouchersController : BaseController
    {
        

        // GET: StudentVouchers
        public ActionResult Index()
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            var studentVouchers = db.StudentVouchers.Include(s => s.StudentRegistration);
            return View(studentVouchers.ToList());
        }

        // GET: StudentVouchers/Details/5
        public ActionResult Details(int id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            StudentVoucher studentVoucher = db.StudentVouchers.Find(id);
            var pmViewModel = new ProfileUserViewModel
            {
                UserGrant = studentVoucher,
                UserGrantList = db.StudentVouchers.Where(a => a.student_ID == studentVoucher.student_ID)
            };

            //var other_grants = db.StudentVouchers.Where(a => a.student_ID == studentVoucher.student_ID);

            //studentVoucher.student_ID
            return View(pmViewModel);
        }

        // GET: StudentVouchers/Add
        public ActionResult Add(String id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            StudentVoucher studentVoucher = new StudentVoucher();
            //voucher.GrantType = db.GrantTypes.ToList();
            //parsing to the view
            //ViewBag.GrantType = Helpers.Helpers.GrantTypes();
            ViewBag.grant_type_id = db.GrantTypes;
            
            // new SelectList(db.GrantTypes, "grant_type_id", "grant_name");
            //ViewBag.grant_type_id = new SelectList(db.GrantTypes, "grant_type_id", "grant_name");
            ViewBag.student_ID = (id != null) ? id : String.Empty;
            studentVoucher.grant_type_id = 0;
            return View(studentVoucher);
        }

        // POST: StudentVouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "student_ID,GrantDescription,GrantValue,DateOfIssue,KuhaFunds,grant_type_id")] StudentVoucher studentVoucher)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            //lets make sure student exists
            if (this.studentExists(studentVoucher.student_ID) == false)
            {
                ModelState.AddModelError("Student_ID", "Student ID Does Not Exist");
                //return View(StudentRegistration);
            }

            //check the required grant requirements have been meet
            GrantType GrantType = db.GrantTypes.Find(studentVoucher.grant_type_id);

           if (GrantType.grant_description == true && (String.IsNullOrEmpty(studentVoucher.GrantDescription)))
               ModelState.AddModelError("GrantDescription", "Grant description must contain details it is required");


           if (GrantType.grant_value == true && studentVoucher.GrantValue <= 0)
               ModelState.AddModelError("GrantValue", "GrantValue needs to be greater than 0");

            
            if (ModelState.IsValid)
            {
                db.StudentVouchers.Add(studentVoucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            //pass back our data
            ViewBag.grant_type_id = db.GrantTypes;
            ViewBag.student_ID = (studentVoucher.student_ID != null) ? studentVoucher.student_ID : String.Empty;

            return View(studentVoucher);
        }
        public JsonResult studentSearch(string query)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();

            var result_id = db.StudentRegistrations.Where(x => x.Student_ID.Contains(query.ToString())).Select(x => new
            {
                        student_id = x.Student_ID,
                        student_name = x.FirstName + " " + x.LastName
            });
            var result_fname = db.StudentRegistrations.Where(x => x.FirstName.Contains(query.ToString())).Select(x => new
            {
                      student_id = x.Student_ID,
                      student_name = x.FirstName + " " + x.LastName
            });
            var result_lname = db.StudentRegistrations.Where(x => x.LastName.Contains(query.ToString())).Select(x => new
            {
                        student_id = x.Student_ID,
                        student_name = x.FirstName + " " + x.LastName
            });

            var result = result_lname.Concat(result_id.Concat(result_fname));

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        

        // GET: StudentVouchers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            StudentRegistrationsModel db = new StudentRegistrationsModel();
           
            
            int id_student_vouchers;
            int.TryParse(id, out id_student_vouchers);
            //var theStudent = (from student in db.StudentVouchers where student.id_student_vouchers == id_student_vouchers select student);
            StudentVoucher theStudent = db.StudentVouchers.Find(id_student_vouchers);
            
            if (theStudent == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.student_ID = theStudent.student_ID;
            //ViewBag.GrantType = Helpers.Helpers.GrantTypes();
            //ViewBag.GrantType = new SelectList(Helpers.Helpers.GrantTypes(), "Value", "Text", theStudent.GrantType);
            ViewBag.GrantType = theStudent.GrantType;
                //new SelectList(db.StudentRegistrations, "Student_ID", "FirstName", studentVoucher.student_ID);
            return View(theStudent);
        }

        // POST: StudentVouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "student_ID,GrantType,GrantDescription,GrantValue,DateOfIssue,id_student_vouchers,KuhaFunds")] StudentVoucher studentVoucher)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            if (ModelState.IsValid)
            {
                StudentVoucher theVoucher = db.StudentVouchers.Find(studentVoucher.id_student_vouchers);
                theVoucher.KuhaFunds = studentVoucher.KuhaFunds;
                theVoucher.GrantType = studentVoucher.GrantType;
                theVoucher.GrantValue = studentVoucher.GrantValue;
                theVoucher.GrantDescription = studentVoucher.GrantDescription;
                theVoucher.DateOfIssue = studentVoucher.DateOfIssue;

               // db.StudentVouchers.Add(theVoucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "FirstName", studentVoucher.student_ID);
            return View(studentVoucher);
        }

        // GET: StudentVouchers/Delete/5
        public ActionResult Delete(int id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentVoucher studentVoucher = db.StudentVouchers.Find(id);
            if (studentVoucher == null)
            {
                return HttpNotFound();
            }
            return View(studentVoucher);
        }

        // POST: StudentVouchers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            StudentVoucher studentVoucher = db.StudentVouchers.Find(id);
            db.StudentVouchers.Remove(studentVoucher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       
    }
}
