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
        private StudentVouchersModel db = new StudentVouchersModel();

        // GET: StudentVouchers
        public ActionResult Index()
        {
            var studentVouchers = db.StudentVouchers.Include(s => s.StudentRegistration);
            return View(studentVouchers.ToList());
        }

        // GET: StudentVouchers/Details/5
        public ActionResult Details(string id)
        {
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

        // GET: StudentVouchers/Create
        public ActionResult Add()
        {
            //parsing to the view
            ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "Fname");
            //@todo move regiration to passing as helpoer
            ViewBag.Grant_Types = Helpers.Helpers.GrantTypes();
            return View();
        }

        // POST: StudentVouchers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Include = "student_ID,GrantType,GrantDescription,GrantValue,DateOfIssue,Refno,KuhaFunds")] StudentVoucher studentVoucher)
        {
            if (ModelState.IsValid)
            {
                db.StudentVouchers.Add(studentVoucher);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "Fname", studentVoucher.student_ID);
            return View(studentVoucher);
        }

        // GET: StudentVouchers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentVoucher studentVoucher = db.StudentVouchers.Find(id);
            if (studentVoucher == null)
            {
                return HttpNotFound();
            }
            ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "Fname", studentVoucher.student_ID);
            return View(studentVoucher);
        }

        // POST: StudentVouchers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "student_ID,GrantType,GrantDescription,GrantValue,DateOfIssue,Refno,KuhaFunds")] StudentVoucher studentVoucher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentVoucher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.student_ID = new SelectList(db.StudentRegistrations, "Student_ID", "Fname", studentVoucher.student_ID);
            return View(studentVoucher);
        }

        // GET: StudentVouchers/Delete/5
        public ActionResult Delete(string id)
        {
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
            StudentVoucher studentVoucher = db.StudentVouchers.Find(id);
            db.StudentVouchers.Remove(studentVoucher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
