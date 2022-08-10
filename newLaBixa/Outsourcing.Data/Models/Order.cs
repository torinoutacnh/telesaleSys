using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            //DateCreated = DateTime.Now;
            CreateDate = DateTime.Now;
        }
        //public int Id { get; set; }
        public int UserDataMappingId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public double LengthKm { get; set; }
        public double TotalPrice { get; set; }
        public double MaterialPrice { get; set; }
        public double SupportPrice { get; set; }
        public double PlatePrice { get; set; }
        public double Discount { get; set; }
        public double OtherPrice { get; set; }
        public double Debt { get; set; }
        public string Repairer { get; set; }
        public string Checker { get; set; }
        public string Casher { get; set; }
        public string Staff { get; set; }
        public int Type { get; set; }
        public string Note { get; set; }
        public int StoreId { get; set; }
        public string PlateNumber { get; set; }
        public string MaterialNumber { get; set; }
        //public string CustomerName { get; set; }
        //public string CustomerAddress { get; set; }
        //public string CustomerPhone{ get; set; }
        //public string CustomerEmail{ get; set; }
        //public int OrderTotal { get; set; }
        //public bool Deleted { get; set; }
        //public DateTime DateCreated { get; set; }
    }
}
