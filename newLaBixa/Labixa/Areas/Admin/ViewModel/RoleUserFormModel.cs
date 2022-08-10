using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labixa.Areas.Admin.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(DataUserValidator))]
    public class RoleUserFormModel
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}