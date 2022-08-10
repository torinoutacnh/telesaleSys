using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.ViewModel
{
    public class LevelFormModel
    {
        public LevelFormModel()
        {
            level = new List<SelectListItem>();
            //CreateDate = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }
        [Display(Name = "Mã Code")]
        public string CodeLevel { get; set; }
        [Display(Name = "Mức độ")]
        public string LevelName { get; set; }
        [Display(Name = "Level Name English")]
        public string LevelNameEng { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Display(Name = "Title English")]
        public string TitleEng { get; set; }
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Display(Name = "Content English")]
        public string ContentEng { get; set; }
        [Display(Name = "Note")]
        public string Noted { get; set; }
        [Display(Name = "Created Date ")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Last Edited Time")]
        public DateTime LastEditedTime { get; set; }
        [Display(Name = "Hiển Thị")]
        public bool IsActive { get; set; }
        [Display(Name = "Deleted")]
        public bool Deleted { get; set; }
        [Display(Name = "Position")]
        public int Position { get; set; }
        [Display(Name = "temp1")]
        public string temp1 { get; set; }
        [Display(Name = "temp2")]
        public string temp2 { get; set; }
        [Display(Name = "temp3")]
        public string temp3 { get; set; }
        [Display(Name = "temp4")]
        public string temp4 { get; set; }
        [Display(Name = "temp5")]
        public string temp5 { get; set; }
        public IEnumerable<SelectListItem> level { get; set; }
        public int? BrandId { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }


    }
    public class LevelValidator : AbstractValidator<LevelFormModel>
    {
        public LevelValidator()
        {
            // RuleFor(x => x.Title).NotNull().WithMessage("fffffff");
            //RuleFor(x => x.Id).NotNull().WithMessage("role không được để trống");
            //RuleFor(x => x.Description).NotNull().WithMessage("Mô Tả Không Được Để Trống");

        }
    }
}