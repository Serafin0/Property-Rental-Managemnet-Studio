using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationFinal.Models;

namespace WebApplicationFinal.Controllers
{
    public class PaymentsController : Controller
    {
        private PropertyProjectManagerEntities db = new PropertyProjectManagerEntities();

        // GET: Payments
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Leas).Include(p => p.Status1).Include(p => p.Tenant);
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.LeaseId = new SelectList(db.Leases, "LeaseId", "LeaseId");
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName");
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PaymentId,TenantId,LeaseId,Amount,PaymentDate,Status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LeaseId = new SelectList(db.Leases, "LeaseId", "LeaseId", payment.LeaseId);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", payment.Status);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", payment.TenantId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.LeaseId = new SelectList(db.Leases, "LeaseId", "LeaseId", payment.LeaseId);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", payment.Status);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", payment.TenantId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PaymentId,TenantId,LeaseId,Amount,PaymentDate,Status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LeaseId = new SelectList(db.Leases, "LeaseId", "LeaseId", payment.LeaseId);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", payment.Status);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", payment.TenantId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
