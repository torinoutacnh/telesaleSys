using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Service;
using Outsourcing.Data.Models;
using Outsourcing.Core.Common;
using Outsourcing.Core.Extensions;
using WebGrease.Css.Extensions;
using Outsourcing.Core.Framework.Controllers;
using Labixa.Helpers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Labixa.Areas.Admin.Controllers
{
    //[Authorize(Roles = ("Admin"))]
    public class DistrictController : BaseController
    {
        readonly IDistrictService _DistrictService;


        public DistrictController(IDistrictService DistrictService)
        {
            _DistrictService = DistrictService;


        }

        public ActionResult Index()
        {
            var list = _DistrictService.GetDistricts().ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var listDistrict = _DistrictService.GetDistricts().ToSelectListItems(-1);
            var list = new DistrictFormModel { district = listDistrict };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(DistrictFormModel newDistrict, bool continueEditing)
        {
            if (ModelState.IsValid)
            {


                var item = Mapper.Map<DistrictFormModel, District>(newDistrict);
                _DistrictService.CreateDistrict(item);
                return continueEditing ? RedirectToAction("Edit", "District", new { id = item.Id })
                                    : RedirectToAction("Index", "District");
            }
            else
            {
                return View("Create", newDistrict);
            }
        }

        public ActionResult Edit(int id)
        {
            var item = _DistrictService.GetDistrictById(id);


            DistrictFormModel obj = Mapper.Map<District, DistrictFormModel>(item);
            if (obj != null)
            {
                return View(obj);
            }
            return RedirectToAction("Index", "District");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(DistrictFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                District item = Mapper.Map<DistrictFormModel, District>(obj);
                _DistrictService.EditDistrict(item);
                return continueEditing ? RedirectToAction("Edit", "District", new { id = item.Id })
                    : RedirectToAction("Index", "District");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _DistrictService.GetDistrictById(id);
            item.IsDeleted = true;
            _DistrictService.EditDistrict(item);
            return RedirectToAction("Index", "District");
        }
    }
}

