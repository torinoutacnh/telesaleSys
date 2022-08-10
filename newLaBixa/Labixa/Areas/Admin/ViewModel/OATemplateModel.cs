using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class OATemplateModel
    {
        public OATemplateModel()
        {
            DateCreated = DateTime.Now;
            LastEditedTime = DateTime.Now;
            IsActive = true;
            Deleted = false;
        }
        [Display(Name = "Tên Template")]
        public string Template_Name { get; set; }
        [Display(Name = "Chế Độ")]
        public string Mode { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }
        public string TemplateId { get; set; }
        [Display(Name = "OA ID")]
        public string TrackingId { get; set; }
        public string Access_Token { get; set; }
        public string Address { get; set; }
        [Display(Name = "Đoạn mã")]
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        [Display(Name = "Hiển thị")]
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public string TeleId { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }

        public Nullable<int> BrandId { get; set; }
        public string Brand_Name { get; set; }
        public string Brand_Code { get; set; }

        public string Balance { get; set; }
        public ICollection<OATemplateAttribute> OATemplateAttributes { get; set; }
    }
}