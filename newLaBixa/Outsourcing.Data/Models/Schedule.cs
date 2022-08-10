using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
   public class Schedule :BaseEntity
    {
        public string Works { get; set; }
        public string Name { get; set; }
        public Nullable<int> Status { get; set; }
        public int UserDataId { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> EditDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool Active { get; set; }
        public string TeleId { get; set; }
        public string TeleName { get; set; }

        public string BienSoXe { get; set; }
        public string LoaiXe { get; set; }
        public string LoaiDichVu { get; set; }
        public Nullable<System.DateTime> NgayGioHen { get; set; }
        public string AddressStore { get; set; }

        [ForeignKey(@"UserDataId")]
        public virtual UserDataMapping UserDataMapping { get; set; }

    }
}
