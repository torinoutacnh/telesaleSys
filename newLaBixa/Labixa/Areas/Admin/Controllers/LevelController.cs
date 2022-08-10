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
    [Authorize(Roles = ("Admin"))]
    public class LevelController : BaseController
    {
        readonly ILevelService _LevelService;
        readonly IBrandService _BrandService;
        readonly IProductService _ProductService;

        int _BRAND_ID = 0;
        string _BRAND_CODE = "THT";
        string _BRAND_NAME = "Toàn Hệ Thống";
        public LevelController(ILevelService LevelService, IBrandService brandService, IProductService productService)
        {
            _LevelService = LevelService;
            _BrandService=brandService;
            _ProductService = productService;

        }
        public void GetCookie()
        {
            HttpCookie brandIdCookie = Request.Cookies["BrandId"];
            HttpCookie brandNameCookie = Request.Cookies["BrandCode"];
            HttpCookie brandCodeCookie = Request.Cookies["BrandName"];
            if (brandIdCookie != null)
            {
                _BRAND_ID = int.Parse(brandIdCookie.Value);
                _BRAND_CODE = brandNameCookie.Value;
                _BRAND_NAME = brandCodeCookie.Value;
            }
        }
        public ActionResult Index()
        {
            GetCookie();
            var list = new List<Level>();
            if (_BRAND_ID != 0)
            {
                list = _LevelService.GetLevels().Where(p => p.BrandId == _BRAND_ID).ToList();
            }
            else
            {
                list = _LevelService.GetLevels().ToList();

            }
            return View(list);
        }

        public ActionResult Create()
        {
            GetCookie();
            var listLevel = _LevelService.GetLevels().ToSelectListItems(-1);
            var list = new LevelFormModel { level = listLevel };
            var listBrand = _BrandService.GetBrands().ToSelectListItems(-1);
            var listProduct = _ProductService.GetProducts().ToSelectListItems(-1); 
            if (_BRAND_ID!=0)
            {
             listProduct = _ProductService.GetProducts().Where(p => p.ProductCategory.BrandId == _BRAND_ID).ToSelectListItems(-1);
            }
            //var listBrand = _BrandService.GetBrands();
            ViewBag.listBrand = listBrand;
            ViewBag.listProduct = listProduct;
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(LevelFormModel newLevel, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                newLevel.DateCreated = DateTime.Now;
                var brand = _BrandService.GetBrandById(int.Parse(newLevel.BrandId.ToString()));
                newLevel.BrandName = brand.Name;
                newLevel.BrandCode = brand.Code;

                var product = _ProductService.GetProductById(newLevel.Position);
                newLevel.temp1 = product.Name;

                var item = Mapper.Map<LevelFormModel, Level>(newLevel);
                _LevelService.CreateLevel(item);
                return continueEditing ? RedirectToAction("Edit", "Level", new { id = item.Id })
                                    : RedirectToAction("Index", "Level");
            }
            else
            {
                return View("Create", newLevel);
            }
        }

        public ActionResult Edit(int id)
        {
            GetCookie();
            var item = _LevelService.GetLevelById(id);
            var listBrand = _BrandService.GetBrands().ToSelectListItems(int.Parse(item.BrandId.ToString()));
            ViewBag.listBrand = listBrand;
            var listProduct = _ProductService.GetProducts().ToSelectListItems(item.Position);
            if (_BRAND_ID != 0)
            {
                listProduct = _ProductService.GetProducts().Where(p => p.ProductCategory.BrandId == _BRAND_ID).ToSelectListItems(item.Position);
            }
            ViewBag.listProduct = listProduct;

            LevelFormModel obj = Mapper.Map<Level, LevelFormModel>(item);
            if (obj != null)
            {
                return View(obj);
            }
            return RedirectToAction("Index", "Level");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(LevelFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                Level item = Mapper.Map<LevelFormModel, Level>(obj);
                var brand = _BrandService.GetBrandById(int.Parse(item.BrandId.ToString()));
                item.BrandName = brand.Name;
                item.BrandCode = brand.Code;
                var product = _ProductService.GetProductById(item.Position);
                item.temp1 = product.Name;
                _LevelService.EditLevel(item);
                return continueEditing ? RedirectToAction("Edit", "Level", new { id = item.Id })
                    : RedirectToAction("Index", "Level");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _LevelService.GetLevelById(id);
            item.Deleted = true;
            _LevelService.EditLevel(item);
            return RedirectToAction("Index", "Level");
        }
    }
}

