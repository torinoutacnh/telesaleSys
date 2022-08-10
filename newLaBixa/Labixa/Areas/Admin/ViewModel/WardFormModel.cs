using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class WardFormModel
    {
        public WardFormModel()
        {
            ward = new List<SelectListItem>();
            DateCreated = new DateTime();
            LastEditedTime = new DateTime();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên Khu Vực")]
        public string Name { get; set; }
        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        [Display(Name = "Ngày Chỉnh Sửa")]
        public System.DateTime LastEditedTime { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string Description { get; set; }
        [Display(Name = "Huyện, Quận")]
        public string District { get; set; }
        [Display(Name = "Tỉnh, TP")]
        public string Province { get; set; }
        [Display(Name = "Mã Code")]
        public string Code { get; set; }
        public IEnumerable<SelectListItem> ward { get; set; }
    }
}