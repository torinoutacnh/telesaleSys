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
    public class BrandController : BaseController
    {
        readonly IBrandService _BrandService;


        public BrandController(IBrandService BrandService)
        {
            _BrandService = BrandService;


        }

        public ActionResult Index()
        {
            var list = _BrandService.GetBrands().ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var listBrand = _BrandService.GetBrands().ToSelectListItems(-1);
            var list = new BrandFormModel { brand = listBrand };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(BrandFormModel newBrand, bool continueEditing)
        {
            if (ModelState.IsValid)
            {


                var item = Mapper.Map<BrandFormModel, Brand>(newBrand);
                _BrandService.CreateBrand(item);
                return continueEditing ? RedirectToAction("Edit", "Brand", new { id = item.Id })
                                    : RedirectToAction("Index", "Brand");
            }
            else
            {
                return View("Create", newBrand);
            }
        }

        public ActionResult Edit(int id)
        {
            var item = _BrandService.GetBrandById(id);


            BrandFormModel obj = Mapper.Map<Brand, BrandFormModel>(item);
            if (obj != null)
            {
                return View(obj);
            }
            return RedirectToAction("Index", "Brand");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(BrandFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                Brand item = Mapper.Map<BrandFormModel, Brand>(obj);
                _BrandService.EditBrand(item);
                return continueEditing ? RedirectToAction("Edit", "Brand", new { id = item.Id })
                    : RedirectToAction("Index", "Brand");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _BrandService.GetBrandById(id);
            item.IsDeleted = true;
            _BrandService.EditBrand(item);
            return RedirectToAction("Index", "Brand");
        }
    }
}

