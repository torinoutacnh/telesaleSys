using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Labixa.Areas.Admin.Controllers
{
    public class ProductCategoryController : Controller
    {
                        #region Field
        readonly IProductCategoryService _productCategoryService;
        readonly IBrandService _brandService;
        #endregion

        #region Ctor
        public ProductCategoryController(IProductCategoryService productCategoryService, IBrandService brandService)
        {
            _productCategoryService = productCategoryService;
            this._brandService = brandService;
        }
        #endregion
        int _BRAND_ID = 0;
        string _BRAND_CODE = "THT";
        string _BRAND_NAME = "Toàn Hệ Thống";
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
        //
        // GET: /Admin/ProductCategory/
        public ActionResult Index()
        {
            GetCookie();
            var list = _productCategoryService.GetProductCategories().Where(p => p.BrandId == _BRAND_ID).ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            GetCookie();
            var listCategory = _productCategoryService.GetProductCategories().Where(p=>p.BrandId==_BRAND_ID).ToSelectListItems(-1);
            ViewBag.ListBrand = _brandService.GetBrands().ToSelectListItems(-1);
            var list = new ProductCategoryFormModel {ListCategory = listCategory };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(ProductCategoryFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                obj.Slug = StringConvert.ConvertShortName(obj.Name);
                var brand = _brandService.GetBrandById(int.Parse(obj.BrandId.ToString()));
                var item = Mapper.Map<ProductCategoryFormModel, ProductCategory>(obj);
                item.Brand_Name = brand.Name;
                item.Brand_Code= brand.Code;
                _productCategoryService.CreateProductCategory(item);
                return continueEditing ? RedirectToAction("Edit", "ProductCategory", new { id = item.Id })
                                    : RedirectToAction("Index", "ProductCategory");
            }
            else
            {
                return View("Create", obj);
            }
        }

        public ActionResult Edit(int id)
        {
            GetCookie();
            var obj = _productCategoryService.GetProductCategoryById(id);
            ViewBag.ListBrand = _brandService.GetBrands().ToSelectListItems(int.Parse(obj.BrandId.ToString()));
            var list2 = _productCategoryService.GetProductCategories().Where(p => p.BrandId == _BRAND_ID).ToSelectListItems(int.Parse(obj.Position.ToString() == "" ? "0" : obj.Position.ToString()));
            var item = Mapper.Map<ProductCategory, ProductCategoryFormModel>(obj);
            item.ListCategory = list2;
            return View(item);
        }

        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(ProductCategoryFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                obj.Slug = StringConvert.ConvertShortName(obj.Name);
                var brand = _brandService.GetBrandById(int.Parse(obj.BrandId.ToString()));
                var item = Mapper.Map<ProductCategoryFormModel, ProductCategory>(obj);
                item.Brand_Name = brand.Name;
                item.Brand_Code = brand.Code;
                _productCategoryService.EditProductCategory(item);
                return continueEditing ? RedirectToAction("Edit", "ProductCategory", new { id = item.Id })
                                   : RedirectToAction("Index", "ProductCategory");
            }
            return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _productCategoryService.GetProductCategoryById(id);
            item.Deleted = true;
            _productCategoryService.EditProductCategory(item);
            return RedirectToAction("Index", "ProductCategory");
        }
	}
}