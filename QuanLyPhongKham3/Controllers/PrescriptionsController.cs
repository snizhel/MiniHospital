using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using QuanLyPhongKham3.Models;

namespace QuanLyPhongKham3.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    public class PrescriptionsController : Controller
    {
        private QLPKEntities db = new QLPKEntities();

        // GET: Prescriptions
        public ActionResult Index()
        {
            string useid =this.User.Identity.GetUserId();
            Staff doctor=db.Staff.Where(x => x.UserId == useid).FirstOrDefault();
            Response.AddHeader("Refresh", "5");
            return View(db.Prescription.Where(x => x.Status == "New" && x.DateOfCreate == DateTime.Today && x.IdStaff==doctor.ID).ToList());
        }

        public ActionResult Add(int patient_id, int doctor_id)
        {
            Customer customer = db.Customer.Find(patient_id);

            Prescription prescription = new Prescription
            {
                IDCustomer = patient_id,
                IdStaff = doctor_id,
                Status = "New",
                DateOfCreate = DateTime.Today,


            };
            try
            {
                db.Prescription.Add(prescription);
                db.SaveChanges();
                return Json(new
                {
                    status = "OK"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "ERROR",
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            ////   ViewBag.Staff = new SelectList(db.AspNetRoles.Where(role => role.Name.Contains("Doctor")).ToList(), "Name", "Name", "Employee");
            //     DoctorCart cart = DoctorCart.getInstance();
            //     cart.Add(item);
            //     //return View(DoctorCart.getInstance().List.Values);
        }


        // GET: Prescriptions
        public ActionResult Apply(int? id)
        {
            Prescription prescription = db.Prescription.Find(id);
            if (prescription == null)
            {
                return HttpNotFound();
            }
            var medicines = db.Medicine.Select(x => new { Id = x.ID, Name = x.Name + " (" + x.Unit + ")" });
            ViewBag.IDMedicine = new SelectList(medicines, "Id", "Name");
            ViewBag.prescription = prescription;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply([Bind(Include = "IDPrescription,IDMedicine,Quantity,Morning,Noon,Afternoon,Night,Using")] PrescriptionDetail detail)
        {

            if (ModelState.IsValid)
            {
                db.PrescriptionDetails.Add(detail);
                db.SaveChanges();
                return RedirectToAction("Apply", new { id = detail.IDPrescription });
            }

            var medicines = db.Medicine.Select(x => new { Id = x.ID, Name = x.Name + " (" + x.Unit + ")" });
            ViewBag.MedicineId = new SelectList(medicines, "Id", "Name");
            ViewBag.prescription = db.Prescription.Find(detail.IDPrescription);
            return View();
        }

        public ActionResult Transfer(int? detail_id, int? pres_id)
        {
            Prescription prescription = db.Prescription.Find(pres_id);
            try
            {
                prescription.Status = "Unpaid";
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new
                {
                    
                    status = "OK",
                }, JsonRequestBehavior.AllowGet);
                

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = "ERROR",
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }


        // GET: Prescriptions/Details/5
        public ActionResult Details(int? pres_id)
        {
            if (pres_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prescription prescription = db.Prescription.Find(pres_id);
            //try
            //{
            //    return Json(new
            //    {
            //        status = "OK"
            //    }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    return Json(new
            //    {
            //        status = "ERROR",
            //        message = ex.Message
            //    }, JsonRequestBehavior.AllowGet);
            //}
            if (prescription == null)
            {
                return HttpNotFound();
            }
            return View(prescription);
        }

        // GET: Prescriptions/Create
        public ActionResult Create()
        {
            ViewBag.PatientId = new SelectList(db.Customer, "Id", "Name");
            ViewBag.DoctorId = new SelectList(db.Staff, "Id", "Name");
            return View();
        }

        // POST: Prescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DoctorId,PatientId,DateOfCreate,Symptom,Diagnosis,Status")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Prescription.Add(prescription);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PatientId = new SelectList(db.Customer, "Id", "Name", prescription.IDCustomer);
            ViewBag.DoctorId = new SelectList(db.Staff, "Id", "Name", prescription.IdStaff);
            return View(prescription);
        }

        // GET: Prescriptions/Edit/5
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
            ViewBag.PatientId = new SelectList(db.Customer, "Id", "Name", prescription.IDCustomer);
            ViewBag.DoctorId = new SelectList(db.Staff, "Id", "Name", prescription.IdStaff);
            return View(prescription);
        }

        // POST: Prescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DoctorId,PatientId,DateOfCreate,Symptom,Diagnosis,Status")] Prescription prescription)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prescription).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientId = new SelectList(db.Customer, "Id", "Name", prescription.IDCustomer);
            ViewBag.DoctorId = new SelectList(db.Staff, "Id", "Name", prescription.IdStaff);
            return View(prescription);
        }

        // GET: Prescriptions/Delete/5
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

        // POST: Prescriptions/Delete/5
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
