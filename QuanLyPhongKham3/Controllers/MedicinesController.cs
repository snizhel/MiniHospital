using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyPhongKham3.Models;
using PagedList;//Must install
namespace QuanLyPhongKham3.Controllers
{
    [Authorize(Roles ="Dispenser,Admin")]
    public class MedicinesController : Controller
    {
        private QLPKEntities db = new QLPKEntities();

        // GET: Medicines
        public ActionResult Index(string searchBy, string search, int? page, string sortBy)
        {

            var medicine = db.Medicine.Include(m => m.MedicineType);
            ViewBag.NameSort = String.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.UnitSort = sortBy == "Unit" ? "Unit desc" : "Unit";
            ViewBag.TypeSort = sortBy == "Type" ? "Type desc" : "Type";
            var medicines = db.Medicine.AsQueryable();

            if (searchBy == "Unit")
            {
                medicines = medicines.Where(x => x.Unit == search || search == null);
            }
            else
            {
                medicines = medicines.Where(x => x.Name.StartsWith(search) || search == null);
            }
            

            switch (sortBy)
            {
                case "Name desc":
                    medicines = medicines.OrderByDescending(x => x.Name);
                    break;
                case "Unit desc":
                    medicines = medicines.OrderByDescending(x => x.Unit);
                    break;
                case "Unit":
                    medicines = medicines.OrderBy(x => x.Unit);
                    break;
                case "Type desc":
                    medicines = medicines.OrderByDescending(x => x.IDMedicineType);
                    break;
                case "Type":
                    medicines = medicines.OrderBy(x => x.IDMedicineType);
                    break;
                default:
                    medicines = medicines.OrderBy(x => x.Name);
                    break;
            }

            return View(medicines.ToPagedList(page ?? 1, 5));
            //return View(medicine.ToList());
        }

        // GET: Medicines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicine.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // GET: Medicines/Create
        public ActionResult Create()
        {
            ViewBag.IDMedicineType = new SelectList(db.MedicineType, "ID", "Name");
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,IDMedicineType,Unit,Count,Cost,Price")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                db.Medicine.Add(medicine);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDMedicineType = new SelectList(db.MedicineType, "ID", "Name", medicine.IDMedicineType);
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicine.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDMedicineType = new SelectList(db.MedicineType, "ID", "Name", medicine.IDMedicineType);
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,IDMedicineType,Unit,Count,Cost,Price")] Medicine medicine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDMedicineType = new SelectList(db.MedicineType, "ID", "Name", medicine.IDMedicineType);
            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicine medicine = db.Medicine.Find(id);
            if (medicine == null)
            {
                return HttpNotFound();
            }
            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicine medicine = db.Medicine.Find(id);
            db.Medicine.Remove(medicine);
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
