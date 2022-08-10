using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class DistrictFormModel
    {
        public DistrictFormModel()
        {
            district = new List<SelectListItem>();

            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên Huyện, TP")]
        public string Name { get; set; }
        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        [Display(Name = "Ngày chỉnh sửa")]
        public System.DateTime LastEditedTime { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        [Display(Name = "Tỉnh")]
        public string Province { get; set; }
        [Display(Name = "Mã Code")]
        public string Code { get; set; }
        public IEnumerable<SelectListItem> district { get; set; }
    }
}