using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class ScheduleFormModel
    {
        public ScheduleFormModel()
        {
            ListSchedule = new List<SelectListItem>();
            ListDataUser = new List<SelectListItem>();
            ListScheduleStore = new List<Store>();
        }

        [Key]
        public int Id { get; set; }
        [DisplayName(@"Công Việc")]
        public string Works { get; set; }
        [DisplayName(@"Tên Lịch")]
        public string Name { get; set; }
        [DisplayName(@"Trạng Thái")]
        public int Status { get; set; }
        [DisplayName(@"Tên Khách Hàng")]
        public int UserDataId { get; set; }

        [DisplayName(@"Biển số xe")]
        public string BienSoXe { get; set; }
        [DisplayName(@"Loại xe")]
        public string LoaiXe { get; set; }
        [DisplayName(@"Loại dịch vụ")]
        public string LoaiDichVu { get; set; }
        [DisplayName(@"Địa cửa hàng")]
        public string AddressStore { get; set; }
        public System.DateTime Deadline { get; set; }
        public System.DateTime CreateDate { get; set; }
        [DisplayName(@"Hiển Thị")]
        public bool IsActive { get; set; }
        [DisplayName(@"Mã Lịch")]
        public string Code { get; set; }
        public System.DateTime EditDate { get; set; }
        public IEnumerable<SelectListItem> ListSchedule { get; set; }
        public IEnumerable<SelectListItem> ListDataUser { get; set; }
        public IEnumerable<Store> ListScheduleStore { get; set; }
        public virtual UserDataMapping UserDataMapping { get; set; }
        public virtual SchedulesStoreMapping SchedulesStore { get; set; }
    }
}