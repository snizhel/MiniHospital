using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyPhongKham3.Models;

namespace QuanLyPhongKham3.Controllers
{
    [Authorize(Roles = "Dispenser,Admin")]
    public class MedicineTypesController : Controller
    {
        private QLPKEntities db = new QLPKEntities();

        // GET: MedicineTypes
        public ActionResult Index()
        {
            return View(db.MedicineType.ToList());
        }

        // GET: MedicineTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineType medicineType = db.MedicineType.Find(id);
            if (medicineType == null)
            {
                return HttpNotFound();
            }
            return View(medicineType);
        }

        // GET: MedicineTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MedicineTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] MedicineType medicineType)
        {
            if (ModelState.IsValid)
            {
                db.MedicineType.Add(medicineType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medicineType);
        }

        // GET: MedicineTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineType medicineType = db.MedicineType.Find(id);
            if (medicineType == null)
            {
                return HttpNotFound();
            }
            return View(medicineType);
        }

        // POST: MedicineTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] MedicineType medicineType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicineType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medicineType);
        }

        // GET: MedicineTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicineType medicineType = db.MedicineType.Find(id);
            if (medicineType == null)
            {
                return HttpNotFound();
            }
            return View(medicineType);
        }

        // POST: MedicineTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicineType medicineType = db.MedicineType.Find(id);
            db.MedicineType.Remove(medicineType);
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
