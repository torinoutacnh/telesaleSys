using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class StoreFormModel
    {
        public StoreFormModel()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
            ListBrands = new List<SelectListItem>();
            ListDistricts = new List<SelectListItem>();
            ListWards = new List<SelectListItem>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên Cửa Hàng")]
        public string Name { get; set; }
        [Display(Name = "Mã Code")]
        public string Store_Code { get; set; }
        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        [Display(Name = "Địa Chỉ")]
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        [Display(Name = "Ngày Chỉnh Sửa")]
        public System.DateTime LastEditedTime { get; set; }
        public string Email { get; set; }
        [Display(Name = "Số Điện Thoại")]
        public Nullable<decimal> PhoneNumber { get; set; }
        public string Description { get; set; }
        [Display(Name = "Quận, Huyện")]
        public string District { get; set; }
        [Display(Name = "Tỉnh, TP")]
        public string Province { get; set; }
        public string Code { get; set; }
        [Display(Name = "Thương Hiệu")]
        public int BrandId { get; set; }
        [Display(Name = "Quận , Huyện")]
        public int DistrictId { get; set; }
        [Display(Name = "Tỉnh , TP")]
        public int WardId { get; set; }
        public IEnumerable<SelectListItem> ListBrands { get; set; }
        public IEnumerable<SelectListItem> ListDistricts { get; set; }
        public IEnumerable<SelectListItem> ListWards{ get; set; }
    }
}