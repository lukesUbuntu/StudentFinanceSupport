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
        public ActionResult Details(string id)
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

        // GET: StudentVouchers/Add
        public ActionResult Add(String id)
        {
            //parsing to the view
            ViewBag.GrantType = Helpers.Helpers.GrantTypes();
            ViewBag.student_ID = (id != null) ? id : String.Empty;
           
            return View();
        }
        // POST: StudentVouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "student_ID,GrantType,GrantDescription,GrantValue,DateOfIssue,KuhaFunds")] StudentVoucher studentVoucher)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            //lets make sure student exists
            if (this.studentExists(studentVoucher.student_ID) == false)
            {
                ModelState.AddModelError("Student_ID", "Student ID Does Not Exist");
                //return View(StudentRegistration);
            }
            //description field is only required value if grant advice selected

            if (studentVoucher.GrantType != null && studentVoucher.GrantType.Contains("Advice")) 
            {
                //Clear the GrantValue error as will be triggered otherwise
                //ModelState["GrantValue"].Errors.Clear();
                ModelState.Remove("GrantValue");
                if (String.IsNullOrEmpty(studentVoucher.GrantDescription))
                {
                    ModelState.AddModelError("GrantDescription", "Grant description must contain details on advice");
                }
                
                
            }

            if (ModelState.IsValid)
            {
                db.StudentVouchers.Add(studentVoucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GrantType = Helpers.Helpers.GrantTypes();
            ViewBag.student_ID = (studentVoucher.student_ID != null) ? studentVoucher.student_ID : String.Empty;
            // ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "FirstName", studentVoucher.student_ID);
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

               // db.StudentVouchers.Add(theVoucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "FirstName", studentVoucher.student_ID);
            return View(studentVoucher);
        }

        // GET: StudentVouchers/Delete/5
        public ActionResult Delete(string id)
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
