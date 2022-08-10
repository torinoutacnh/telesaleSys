using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(LogDiaryValidator))]
    public class LogDiaryFormModel
    {
        [Key]
        public int Id { get; set; }
        public string CodeLogDiary { get; set; }
        public string LogName { get; set; }
        public string LogNameEng { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        public string Noted { get; set; }
        public int TotalCallSucceed { get; set; }
        public int TotalNotCall { get; set; }
        public int TotalCallAgain { get; set; }
        public int TotalCall { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> LastEditedTime { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Position { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public int UserDataID { get; set; }
        public string TimeCall { get; set; }
        public string StatusCall { get; set; }
        public int TotalStopCall { get; set; }
        public string LinkFile { get; set; }
        public string PhoneReceived { get; set; }
        public int LevelId { get; set; }
        public int Price_1 { get; set; }
        public int Price_2 { get; set; }
        public int Price_3 { get; set; }
    }
    public class LogDiaryValidator : AbstractValidator<LogDiaryFormModel>
    {
        public LogDiaryValidator()
        {
            // RuleFor(x => x.Title).NotNull().WithMessage("fffffff");
            //RuleFor(x => x.Id).NotNull().WithMessage("role không được để trống");
            //RuleFor(x => x.Description).NotNull().WithMessage("Mô Tả Không Được Để Trống");

        }
    }
}