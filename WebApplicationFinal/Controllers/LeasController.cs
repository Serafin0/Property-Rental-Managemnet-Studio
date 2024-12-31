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
    public class LeasController : Controller
    {
        private PropertyProjectManagerEntities db = new PropertyProjectManagerEntities();

        // GET: Leas
        public ActionResult Index(int? statusFilter)
        {
            // Retrieve user information from the session
            var userType = Session["UserType"]?.ToString(); // e.g., "Manager", "Owner", or "Tenant"
            var userId = Session["UserID"]; // ID of the logged-in user

            if (userType == null || userId == null)
            {
                // Redirect to login if session information is missing
                return RedirectToAction("Login", "Account");
            }

            var leases = db.Leases.Include(l => l.Apartment).Include(l => l.Status1).Include(l => l.Tenant);

            if (userType == "Tenant")
            {
                // Cast userId to int and filter leases for the specific tenant
                int tenantId = Convert.ToInt32(userId);
                leases = leases.Where(l => l.TenantId == tenantId);
            }

            // Apply filter if a status is selected
            if (statusFilter.HasValue)
            {
                leases = leases.Where(l => l.Status1.StatusId == statusFilter.Value);
            }

            // Pass the selected status to the ViewBag to retain dropdown selection
            ViewBag.StatusFilter = statusFilter;

            return View(leases.ToList());
        }

        // GET: Leas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leas leas = db.Leases.Find(id);
            if (leas == null)
            {
                return HttpNotFound();
            }
            return View(leas);
        }

        // GET: Leas/Create
        public ActionResult Create()
        {
            ViewBag.ApartmentNumber = new SelectList(db.Apartments, "ApartmentNumber", "ApartmentNumber");
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName");
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName");
            return View();
        }

        // POST: Leas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeaseId,TenantId,ApartmentNumber,StartDate,EndDate,Status")] Leas leas)
        {
            if (ModelState.IsValid)
            {
                db.Leases.Add(leas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApartmentNumber = new SelectList(db.Apartments, "ApartmentNumber", "ApartmentNumber", leas.ApartmentNumber);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", leas.Status);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", leas.TenantId);
            return View(leas);
        }

        // GET: Leas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leas leas = db.Leases.Find(id);
            if (leas == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApartmentNumber = new SelectList(db.Apartments, "ApartmentNumber", "ApartmentNumber", leas.ApartmentNumber);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", leas.Status);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", leas.TenantId);
            return View(leas);
        }

        // POST: Leas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeaseId,TenantId,ApartmentNumber,StartDate,EndDate,Status")] Leas leas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApartmentNumber = new SelectList(db.Apartments, "ApartmentNumber", "ApartmentNumber", leas.ApartmentNumber);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", leas.Status);
            ViewBag.TenantId = new SelectList(db.Tenants, "TenantId", "FirstName", leas.TenantId);
            return View(leas);
        }

        // GET: Leas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leas leas = db.Leases.Find(id);
            if (leas == null)
            {
                return HttpNotFound();
            }
            return View(leas);
        }

        // POST: Leas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leas leas = db.Leases.Find(id);
            db.Leases.Remove(leas);
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
