using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.Models
{
    public class LogHistory
    {
        public int stt { get; set; }
        public string key { get; set; }
        public string ngayGoi { get; set; }
        public string soGoiDen { get; set; }
        public string dauSo { get; set; }
        public string soNhan { get; set; }
        public string trangThai { get; set; }
        public string tongThoiGianGoi { get; set; }
        public string thoiGianThucGoi { get; set; }
        public string linkFile { get; set; }
    }
}