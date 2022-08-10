using FluentValidation;
using Labixa.App_Data;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(DataUserValidator))]
    public class DataUserFormModel
    {

        [Key]
        public int Id { get; set; }
        [Display(Name = "Mã Khách Hàng")]
        public string CodeData { get; set; }
        [Display(Name = "Tên Khách Hàng")]
        public string FirstName { get; set; }
        [Display(Name = "Họ KH")]
        public string LastName { get; set; }
        [Display(Name = "Tên Họ KH")]
        public string UserName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Ngày sinh nhật")]
        public Nullable<System.DateTime> DayOfBith { get; set; }
        [Display(Name = "Giới tính")]
        public Nullable<bool> Sex { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        [Display(Name = "Ghi chú")]
        public string Noted { get; set; }
        public Nullable<int> TotalOrder { get; set; }
        public Nullable<int> TotalInteractive { get; set; }
        [Display(Name = "Ngày tạo")]
        public Nullable<System.DateTime> DateCreated { get; set; }
        [Display(Name = "Ngày chỉnh sửa")]
        public Nullable<System.DateTime> LastEditedTime { get; set; }
        [Display(Name = "Hiển thị")]
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public Nullable<int> Position { get; set; }
        [Display(Name = "Kênh Khách hàng")]
        public int ChannelDataId { get; set; }
        [Display(Name = "Level khách hàng")]
        public int LevelId { get; set; }
        public Nullable<int> TotalCallSucess { get; set; }
        public Nullable<int> TotalNotCall { get; set; }
        public Nullable<int> TotalCallAgains { get; set; }
        public Nullable<int> TotalCallRefuse { get; set; }
        public int StoreId { get; set; }
        [Display(Name = "Doanh Thu")]
        public Nullable<double> Price_1 { get; set; }
        
        public Nullable<double> Price_2 { get; set; }
        public Nullable<double> Price_3 { get; set; }
        public string Zalo { get; set; }
        public string Facebook { get; set; }
        public string ParentName { get; set; }
        public Nullable<int> RelativeId { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }

        [Display(Name = "BrandId")]
        public Nullable<int> BrandId { get; set; }
        [Display(Name = "Brand_Name")]
        public string Brand_Name { get; set; }
        [Display(Name = "Brand_Code")]
        public string Brand_Code { get; set; }

        [Display (Name ="Mã Vé")]
        public string MaVe { get; set; }
        [Display(Name = "Lộ trình")]
        public string TenChang { get; set; }
        [Display(Name = "Chiều")]
        public string Chieu { get; set; }
        [Display(Name = "Hình Thức")]
        public string HinhThuc { get; set; }
        [Display(Name = "Loại Xe")]
        public string LoaiXe { get; set; }
        [Display(Name = "CMND")]
        public string CMND { get; set; }
        [Display(Name = "Điện Thoại Người Đi")]
        public string Phone_NguoiDi { get; set; }
        [Display(Name = "Tên Tài Xế")]
        public string TenTaiXe { get; set; }
        [Display(Name = "Biển Số")]
        public string BienSo { get; set; }
        [Display(Name = "Hình Thức Thanh Toán")]
        public string HT_ThanhToan { get; set; }
        [Display(Name = "Khách Hàng Ghi Chú")]
        public string Partner_Noted { get; set; }
        [Display(Name = "Không Ghép Được")]
        public Nullable<bool> Khong_Ghep_Duoc { get; set; }
        [Display(Name = "Phản Hồi")]
        public string PhanHoi { get; set; }
        [Display(Name = "Tên Miền")]
        public string Ten_Mien { get; set; }
        [Display(Name = "Quảng Đường")]
        public string QuangDuong { get; set; }
        [Display(Name = "Tiết Kiệm")]
        public string TietKiem { get; set; }
        [Display(Name = "Thời Gian Đặt")]
        public DateTime? Thoi_Gian_Dat { get; set; }
        [Display(Name = "Khách Tự Đặt")]
        public Nullable<bool> Khach_Tu_Dat { get; set; }
        [Display(Name = "Đối Tác Vận Chuyển")]
        public string Doi_Tac_Van_Chuyen { get; set; }
        [Display(Name = "Mã Giảm Giá")]
        public string Ma_Giam_Gia { get; set; }
        [Display(Name = "Giá Gốc")]
        public double? Gia_Goc { get; set; }
        [Display(Name = "Lý Do Hủy")]
        public string Ly_Do_Huy { get; set; }
        [Display(Name = "Thời Gian Hủy")]
        public DateTime? Thoi_Gian_Huy { get; set; }
        [Display(Name = "Hãng Khách Chọn")]
        public string Hang_Khach_Chon { get; set; }
        [Display(Name = "Mã Vé Đối Tác")]
        public string Ma_Ve_Doi_Tac { get; set; }
        [Display(Name = "Tình Trạng")]
        public string Tinh_Trang { get; set; }

        public virtual ICollection<CampaignDataMapping> CampaignDataMappings { get; set; }
        public virtual ChanelData ChanelData { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<Logdiary> LogDiaries { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<UserDataMapping> UserDataMappings { get; set; }
    }
    public class DataUserValidator : AbstractValidator<DataUserFormModel>
    { 
        public DataUserValidator()
        {
            // RuleFor(x => x.Title).NotNull().WithMessage("fffffff");
            //RuleFor(x => x.Id).NotNull().WithMessage("role không được để trống");
            //RuleFor(x => x.Description).NotNull().WithMessage("Mô Tả Không Được Để Trống");

        }
    }
}