using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outsourcing.Data.Models
{
    public class DataUser :BaseEntity
    {
        public string CodeData { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> DayOfBith { get; set; }
        public Nullable<bool> Sex { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public Nullable<int> TotalOrder { get; set; }
        public Nullable<int> TotalInteractive { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public Nullable<int> Position { get; set; }
        public int ChannelDataId { get; set; }
        public int LevelId { get; set; }
        public Nullable<int> TotalCallSucess { get; set; }
        public Nullable<int> TotalNotCall { get; set; }
        public Nullable<int> TotalCallAgains { get; set; }
        public Nullable<int> TotalCallRefuse { get; set; }
        public int StoreId { get; set; }
        public string Store_Address { get; set; }
        public string Store_Name { get; set; }
        public string Store_Code { get; set; }
        public string Store_Phone { get; set; }
        public Nullable<double> Price_1 { get; set; }
        public Nullable<double> Price_2 { get; set; }
        public Nullable<double> Price_3 { get; set; }
        public string Zalo { get; set; }
        public string Facebook { get; set; }
        public string ParentName { get; set; }
        public Nullable<int> RelativeId { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public DateTime? LastCall { get; set; }

        public Nullable<int> BrandId { get; set; }
        public string Brand_Name { get; set; }
        public string Brand_Code { get; set; }

        public string MaVe { get; set; }
        public string TenChang { get; set; }
        public string Chieu { get; set; }
        public string HinhThuc { get; set; }
        public string LoaiXe { get; set; }
        public string CMND { get; set; }
        public string Phone_NguoiDi { get; set; }
        public string TenTaiXe { get; set; }
        public string BienSo { get; set; }
        public string HT_ThanhToan { get; set; }
        public string Partner_Noted { get; set; }
        public Nullable<bool> Khong_Ghep_Duoc { get; set; }
        public string PhanHoi { get; set; }
        public string Ten_Mien { get; set; }
        public string QuangDuong { get; set; }
        public string TietKiem { get; set; }
        public DateTime? Thoi_Gian_Dat { get; set; }
        public Nullable<bool> Khach_Tu_Dat { get; set; }
        public string Doi_Tac_Van_Chuyen { get; set; }
        public string Ma_Giam_Gia { get; set; }
        public double? Gia_Goc { get; set; }
        public string Ly_Do_Huy { get; set; }
        public DateTime? Thoi_Gian_Huy { get; set; }
        public string Hang_Khach_Chon { get; set; }
        public string Ma_Ve_Doi_Tac { get; set; }
        public string Tinh_Trang { get; set; }



        public virtual ICollection<CampaignDataMapping> CampaignDataMappings { get; set; }
        public ICollection<AspNetUser> LiAspNetUser { get; set; }
        [ForeignKey("ChannelDataId")]
        public virtual ChanelData ChanelData { get; set; }
        [ForeignKey("LevelId")]
        public virtual Level Level { get; set; }
        public virtual ICollection<Logdiary> LogDiaries { get; set; }
        public virtual ICollection<UserDataMapping> UserDataMappings { get; set; }
    }
}
