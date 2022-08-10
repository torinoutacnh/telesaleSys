using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class BrandFormModel
    {
        public BrandFormModel()
        {
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
            Num1 = Num2 = Num3 = 0;
            Price1 = Price2 = Price3 = 0;
            brand = new List<SelectListItem>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên Thương Hiệu")]
        public string Name { get; set; }
        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
          [Display(Name = "ĐỊa Chỉ")]
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        [Display(Name = "Ngày Chỉnh Sửa")]
        public System.DateTime LastEditedTime { get; set; }
        [Display(Name = "Mô Tả")]
        public string Description { get; set; }
        [Display(Name = "Mã Code")]
        public string Code { get; set; }
        [Display(Name = "Tên Quản Lý")]
        public string ManagerName { get; set; }
        [Display(Name = "Số Điện Thoại")]
        public string PhoneContact { get; set; }
        public string Email { get; set; }
        [Display(Name = "Website")]
        public string Website { get; set; }

        public int Num1 { get; set; }
        public int Num2 { get; set; }
        public int Num3 { get; set; }
        public double Price1 { get; set; }
        public double Price2 { get; set; }
        public double Price3 { get; set; }
        public string Temp1 { get; set; }
        public string Temp2 { get; set; }
        public string Temp3 { get; set; }
        public IEnumerable<SelectListItem> brand { get; set; }
    }
}