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
    [Authorize(Roles = "Admin,Dispenser")]
    public class MedicineKeeperController : Controller
    {
        private QLPKEntities db = new QLPKEntities();

        // GET: MedicineKeeper
        public ActionResult Index()
        {
            var prescription = db.Prescription.Where(x=>x.Status=="Unpaid" );
            return View(prescription.ToList());
        }

        // GET: MedicineKeeper/Details/5
        public ActionResult Details(int? id,[Bind(Include = "IDPrescription,IDMedicine,Quantity,Morning,Noon,Afternoon,Night,Using")] PrescriptionDetail detail)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
            Prescription prescription = db.Prescription.Find(id);
            //ViewBag.prescription = db.Prescription.Find(detail.IDPrescription);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }


        public ActionResult Checkout(int? pres_id)
        {
            Prescription prescription = db.Prescription.Find(pres_id);
            {
                prescription.Status = "Close";
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        // GET: MedicineKeeper/Create
        public ActionResult Create()
        {
            ViewBag.IDCustomer = new SelectList(db.Customer, "ID", "Name");
            ViewBag.IdStaff = new SelectList(db.Staff, "ID", "UserId");
            return View();
        }

        // POST: MedicineKeeper/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdStaff,IDCustomer,DateOfCreate,Symptom,Diagnosis,Status")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Prescription.Add(prescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDCustomer = new SelectList(db.Customer, "ID", "Name", prescription.IDCustomer);
            ViewBag.IdStaff = new SelectList(db.Staff, "ID", "UserId", prescription.IdStaff);
            return View(prescription);
        }

        // GET: MedicineKeeper/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescription.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDCustomer = new SelectList(db.Customer, "ID", "Name", prescription.IDCustomer);
            ViewBag.IdStaff = new SelectList(db.Staff, "ID", "UserId", prescription.IdStaff);
            return View(prescription);
        }

        // POST: MedicineKeeper/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IdStaff,IDCustomer,DateOfCreate,Symptom,Diagnosis,Status")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDCustomer = new SelectList(db.Customer, "ID", "Name", prescription.IDCustomer);
            ViewBag.IdStaff = new SelectList(db.Staff, "ID", "UserId", prescription.IdStaff);
            return View(prescription);
        }

        // GET: MedicineKeeper/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescription.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // POST: MedicineKeeper/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prescription prescription = db.Prescription.Find(id);
            db.Prescription.Remove(prescription);
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
