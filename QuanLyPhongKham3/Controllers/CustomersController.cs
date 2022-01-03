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
    [Authorize(Roles ="Admin,Employee")]
    public class CustomersController : Controller
    {
        private QLPKEntities db = new QLPKEntities();

        // GET: Customers
       
      //  public ActionResult Index()
      //  {
//return View(db.Customer.ToList());
      //  }
        public ActionResult Index(int? seraching, string searchBy, string search, int? page, string sortBy)
        {
            ViewBag.NameSort = String.IsNullOrEmpty(sortBy) ? "Name desc" : "";
            ViewBag.SexSort = sortBy == "Sex" ? "Sex desc" : "Sex";
            ViewBag.PhoneSort = sortBy == "Phone" ? "Phone desc" : "Phone";
            ViewBag.IdStaff = new SelectList(db.Staff.Where(x => x.Type.Contains("Doctor")).ToList(), "ID", "Name");
            var customers = db.Customer.AsQueryable();

            if (searchBy == "Sex")
            {
                customers = customers.Where(x => x.Sex == search || search == null);
            }
            else
            {
                customers = customers.Where(x => x.Name.StartsWith(search) || search == null);
            }
            if(seraching.HasValue)
            {
                return View(db.Customer.Where(x => (x.PhoneNumber == seraching) || search == null));
            }
            
            switch (sortBy)
            {
                case "Name desc":
                    customers = customers.OrderByDescending(x => x.Name);
                    break;
                case "Sex desc":
                    customers = customers.OrderByDescending(x => x.Sex);
                    break;
                case "Sex":
                    customers = customers.OrderBy(x => x.Sex);
                    break;
                default:
                    customers = customers.OrderBy(x => x.Name);
                    break;
            }

            return View(customers.ToPagedList(page ?? 1, 5));
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

       

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,DateOfBirth,Address,PhoneNumber,Sex,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customer.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,DateOfBirth,Address,PhoneNumber,Sex,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customer.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customer.Find(id);
            db.Customer.Remove(customer);
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
