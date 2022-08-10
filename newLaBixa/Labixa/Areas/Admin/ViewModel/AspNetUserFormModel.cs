using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.ViewModel
{
    public class AspNetUserFormModel
    {
        public string Id { get; set; }
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public bool Activated { get; set; }
        public int Gender { get; set; }
        public int RoleId { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public bool Deleted { get; set; }
        public string Email { get; set; }
        public Nullable<bool> EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<bool> PhoneNumberConfirmed { get; set; }
        public Nullable<bool> TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public Nullable<int> AccessFailedCount { get; set; }
        [Display(Name = "Nhân viên phụ trách")] 
        public string UserName { get; set; }
        public Nullable<int> Extention { get; set; }
        public string Dealine { get; set; }
        public Nullable<int> BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string Temp_1 { get; set; }
        public string Temp_2 { get; set; }
        public string Temp_3 { get; set; }
        public string Temp_4 { get; set; }
        public string Temp_5 { get; set; }

        public virtual ICollection<AspNetUserFormModel> liAspNetUser { get; set; }
    }
}