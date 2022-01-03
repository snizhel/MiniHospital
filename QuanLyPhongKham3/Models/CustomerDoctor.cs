using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuanLyPhongKham3.Models
{
    public class CustomerDoctor
    {
        public int ID { get; set; }
        public int IdStaff { get; set; }
        public int IDCustomer { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfCreate { get; set; }
        public string Symptom { get; set; }
        public string Diagnosis { get; set; }
        public string Status { get; set; }

      
    }
}