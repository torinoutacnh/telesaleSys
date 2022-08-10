using FluentValidation;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(UserDataMappingValidator))]
    public class UserDataMappingFormModel
    {
        public UserDataMappingFormModel()
        {
            ListChanel = new List<SelectListItem>();
            ListLevel = new List<SelectListItem>();
            liQuestion = new List<Outsourcing.Data.Models.Question>();
            //   ListLogDiary = new List<Logdiary>();
        }
        [Key]
        public int Id { get; set; }
        public int DataUserId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Position { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public string UserId { get; set; }
        public string noteSchedule { get; set; }
        public Nullable<System.DateTime> ScheduleCall { get; set; }

        public virtual AspNetUserFormModel User { get; set; }
        public virtual DataUserFormModel DataUser { get; set; }
        public List<Outsourcing.Data.Models.Question> liQuestion { get; set; }
        
        //    public virtual ICollection<Schedule> Schedules { get; set; }
        public IEnumerable<Product> ListProduct { get; set; }
        public IEnumerable<SelectListItem> ListChanel { get; set; }
        public IEnumerable<SelectListItem> ListLevel { get; set; }
    //    public List<Logdiary> ListLogDiary { get; set; }
    }
    public class UserDataMappingValidator : AbstractValidator<UserDataMappingFormModel>
    {
        public UserDataMappingValidator()
        {
            // RuleFor(x => x.Title).NotNull().WithMessage("fffffff");
            //RuleFor(x => x.Id).NotNull().WithMessage("role không được để trống");
            //RuleFor(x => x.Description).NotNull().WithMessage("Mô Tả Không Được Để Trống");

        }
    }
}