﻿using System;
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
    public class ApartmentsController : Controller
    {
        private PropertyProjectManagerEntities db = new PropertyProjectManagerEntities();

        // GET: Apartments
        public ActionResult Index()
        {
            var apartments = db.Apartments.Include(a => a.Building).Include(a => a.PropertyManager).Include(a => a.Status1);
            return View(apartments.ToList());
        }

        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // Restrict tenants from accessing Create
        // GET: Apartments/Create
        public ActionResult Create()
        {
            if (Session["UserType"]?.ToString() == "Tenant") 
            {
                return RedirectToAction("Index");
            }

            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName");
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName");
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName");
            return View();
        }



        // POST: Apartments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApartmentNumber,Bedroom,Bathroom,Squarefeet,Price,Status,BuildingId,ManagerId")] Apartment apartment)
        {

            if (Session["UserType"]?.ToString() == "Tenant")
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                db.Apartments.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName", apartment.ManagerId);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", apartment.Status);
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserType"]?.ToString() == "Tenant")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName", apartment.ManagerId);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", apartment.Status);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApartmentNumber,Bedroom,Bathroom,Squarefeet,Price,Status,BuildingId,ManagerId")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BuildingId = new SelectList(db.Buildings, "BuildingId", "BuildingName", apartment.BuildingId);
            ViewBag.ManagerId = new SelectList(db.PropertyManagers, "ManagerId", "FirstName", apartment.ManagerId);
            ViewBag.Status = new SelectList(db.Statuses, "StatusId", "StatusName", apartment.Status);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserType"]?.ToString() == "Tenant")
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserType"]?.ToString() == "Tenant")
            {
                return RedirectToAction("Index");
            }

            Apartment apartment = db.Apartments.Find(id);
            db.Apartments.Remove(apartment);
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
