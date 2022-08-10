using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            Status = 0;
            OA_IdTemplate = 0;
        }
        public int ProductId { get; set; }
        public int LogdiaryId { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string Temp_1 { get; set; }
        public string Temp_2 { get; set; }
        public string Temp_3 { get; set; }
        public string OTP { get; set; }
        public DateTime? DateCreateOTP { get; set; }
        public bool isConfirmed {get;set;}
        public int Status { get; set; }
        public int StoreId { get; set; }
        public int OA_IdTemplate { get; set; }
        public int StoreName { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("LogdiaryId")]
        public virtual Logdiary Logdiary { get; set; }
    }
}
