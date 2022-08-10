using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class ChanelDataFormModel
    {
        public ChanelDataFormModel()
        {
            ListChanel = new List<SelectListItem>();
            ListSource = new List<SelectListItem>();
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Mã Kênh")]
        public string Code { get; set; }
        [Display(Name = "Tên Kênh")]
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        [Display(Name = "Ghi Chú")]
        public string Noted { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastEditedTime { get; set; }
        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Position { get; set; }
        [Display(Name ="Chi phí dự kiến")]
        public string temp1 { get; set; }
        [Display(Name = "Doanh thu dự kiến")]
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        [Display(Name = "Nguồn Khách Hàng")]
        public int SourceDataID { get; set; }
        public IEnumerable<SelectListItem> ListChanel { get; set; }
        public IEnumerable<SelectListItem> ListSource { get; set; }
        public virtual SourceData SourceData { get; set; }
    }
}