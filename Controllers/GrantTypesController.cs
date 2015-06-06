using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentFinanceSupport.Models;

namespace StudentFinanceSupport.Controllers
{
    public class GrantTypesController : BaseController
    {
        private StudentRegistrationsModel db = new StudentRegistrationsModel();

        // GET: GrantTypes
        public ActionResult Index()
        {
            return View(db.GrantTypes.ToList());
        }

        // GET: GrantTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantType grantType = db.GrantTypes.Find(id);
            if (grantType == null)
            {
                return HttpNotFound();
            }
            return View(grantType);
        }

        // GET: GrantTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GrantTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "grant_type_id,grant_name,grant_value,grant_description,grant_koha,system_asset")] GrantType grantType)
        {
            if (ModelState.IsValid)
            {
                db.GrantTypes.Add(grantType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grantType);
        }

        // GET: GrantTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantType grantType = db.GrantTypes.Find(id);
            if (grantType == null)
            {
                return HttpNotFound();
            }
            return View(grantType);
        }

        // POST: GrantTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "grant_type_id,grant_name,grant_value,grant_description,grant_koha,system_asset")] GrantType grantType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grantType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grantType);
        }

        // GET: GrantTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GrantType grantType = db.GrantTypes.Find(id);
            if (grantType == null)
            {
                return HttpNotFound();
            }
            return View(grantType);
        }

        // POST: GrantTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GrantType grantType = db.GrantTypes.Find(id);
            db.GrantTypes.Remove(grantType);
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
