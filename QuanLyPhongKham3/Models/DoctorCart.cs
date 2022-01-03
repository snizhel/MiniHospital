using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyPhongKham3.Models
{
    public class DoctorCart
    {
        public static DoctorCart getInstance()
        {
            return new DoctorCart();
        }
        public SortedList<int, Prescription> List
        {
            get
            {
                HttpSessionStateBase Session = new HttpSessionStateWrapper(HttpContext.Current.Session);
                if (Session["list"] == null)
                    Session["list"] = new SortedList<int, Prescription>();
                return Session["list"] as SortedList<int, Prescription>;
            }
        }
        public void Add(Prescription customer)
        {

            List.Add(customer.ID, customer);
        }

    }
}