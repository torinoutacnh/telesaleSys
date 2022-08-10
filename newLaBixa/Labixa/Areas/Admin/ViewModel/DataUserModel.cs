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
    public class DataUserModel : BaseEntity
    {
        [Key]
        //public int Id { get; set; }
        [Display(Name = "Cửa Hàng")]
        public string StoreTemp { get; set; }
        [Display(Name = "Format số phiếu")]
        public string FormatNumberVotes { get; set; }
        [Display(Name = "Tên KH")]
        public string FullName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }
        [Display(Name = "Thành phố/Tỉnh")]
        public string City { get; set; }
        [Display(Name = "Giới tính")]
        public Nullable<bool> Sex { get; set; }
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Display(Name = "Số KM đã chạy")]
        public string NumberOfKilometer { get; set; }
        [Display(Name = "Biển số xe")]
        public string LicensePlate { get; set; }
        [Display(Name = "Tổng tiền phụ tùng")]
        public string MaterialPrice { get; set; }
        [Display(Name = "Tiền công")]
        public string Fee { get; set; }
        [Display(Name = "Tổng tiền")]
        public string TotalMoney { get; set; }
        [Display(Name = "Người sửa")]
        public string Repairer { get; set; }
        [Display(Name = "Người kiểm")]
        public string Checker { get; set; }
        [Display(Name = "Thu ngân")]
        public string Cashier { get; set; }
        [Display(Name = "Người tiếp nhận")]
        public string Reciever { get; set; }
        [Display(Name = "Yêu cầu của khách hàng")]
        public string CustomerRequest { get; set; }
        [Display(Name = "Người phụ trách")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Noted { get; set; }
        public Nullable<double> Price_1 { get; set; }
        public Nullable<int> Position { get; set; }

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

        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<CampaignDataMapping> CampaignDataMappings { get; set; }
        public virtual ChanelData ChanelData { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<Logdiary> LogDiaries { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<UserDataMapping> UserDataMappings { get; set; }
    }

    public class DataUsersModel : BaseEntity
    {
        [Key]
        //public int Id { get; set; }
        [Display(Name = "Cửa Hàng")]
        public string StoreTemp { get; set; }
        [Display(Name = "Ngày Mua")]
        public string DateBuy { get; set; }
        [Display(Name = "Tên KH")]
        public string FullName { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Phường")]
        public string Ward { get; set; }
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }
        [Display(Name = "Thành phố/Tỉnh")]
        public string City { get; set; }
        [Display(Name = "Ngày sinh")]
        public string DateOfBirth { get; set; }
        [Display(Name = "Giới tính")]
        public Nullable<bool> Sex { get; set; }
        [Display(Name = "Công việc")]
        public string Jobs { get; set; }
        [Display(Name = "Model")]
        public string Model { get; set; }
        [Display(Name = "Màu")]
        public string Color { get; set; }
        [Display(Name = "Số khung")]
        public string FrameNumber { get; set; }
        [Display(Name = "Số máy")]
        public string MachineNumber { get; set; }
        [Display(Name = "Nhân viên bán")]
        public string SaleStaff { get; set; }
        [Display(Name = "Giá đề xuất")]
        public string SuggestedPrice { get; set; }
        [Display(Name = "Giá bán")]
        public string Price { get; set; }

        //[Display(Name = "Số KM đã chạy")]
        //public string NumberOfKilometer { get; set; }
        //[Display(Name = "Biển số xe")]
        //public string LicensePlate { get; set; }
        //[Display(Name = "Tổng tiền phụ tùng")]
        //public string TotalMoneyAccessories { get; set; }
        //[Display(Name = "Tổng cộng")]
        //public string Fee { get; set; }
        [Display(Name = "Tổng tiền")]
        public string TotalMoney { get; set; }

        //new
        [Display(Name = "Tiền biển số")]
        public string PricePlate { get; set; }
        [Display(Name = "Giảm giá")]
        public string SaleOff { get; set; }
        [Display(Name = "Chi phí phát sinh")]
        public string OtherPrice { get; set; }
        [Display(Name = "Số tiền còn phải trả")]
        public string NeedPay { get; set; }
        [Display(Name = "Tổng tiền trả")]
        public string TotalPay { get; set; }
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }


        //[Display(Name = "Người sửa")]
        //public string Repairer { get; set; }
        //[Display(Name = "Người kiểm")]
        //public string Checker { get; set; }
        //[Display(Name = "Thu ngân")]
        //public string Cashier { get; set; }
        //[Display(Name = "Người tiếp nhận")]
        //public string Reciever { get; set; }
        //[Display(Name = "Yêu cầu của khách hàng")]
        //public string CustomerRequest { get; set; }

        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<CampaignDataMapping> CampaignDataMappings { get; set; }
        public virtual ChanelData ChanelData { get; set; }
        public virtual Level Level { get; set; }
        public virtual ICollection<Logdiary> LogDiaries { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<UserDataMapping> UserDataMappings { get; set; }
    }
}