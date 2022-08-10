using FluentValidation;
using Outsourcing.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(SubjectValidator))]
    public class SourceDataFormModel
    {
        public SourceDataFormModel()
        {
            SourceData = new List<SelectListItem>();
            //CreateDate = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [DisplayName(@"Mã Nguồn Khách Hàng")]
        public string Code { get; set; }
        [DisplayName(@"Nguồn Khách Hàng")]
        public string Name { get; set; }
        public string NameEng { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string TitleEng { get; set; }
        public string Content { get; set; }
        public string ContentEng { get; set; }
        [DisplayName(@"Ghi Chú")]
        public string Noted { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastEditedTime { get; set; }
        [DisplayName(@"Hiển Thị")]
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public int Position { get; set; }
        public string temp1 { get; set; }
        public string temp2 { get; set; }
        public string temp3 { get; set; }
        public string temp4 { get; set; }
        public string temp5 { get; set; }
        public IEnumerable<SelectListItem> SourceData { get; set; }

    }
    public class SubjectValidator : AbstractValidator<SourceDataFormModel>
    {
        public SubjectValidator()
        {
            // RuleFor(x => x.Title).NotNull().WithMessage("fffffff");
            RuleFor(x => x.Name).NotNull().WithMessage("Name không được để trống");
            //RuleFor(x => x.Description).NotNull().WithMessage("Mô Tả Không Được Để Trống");

        }
    }
}