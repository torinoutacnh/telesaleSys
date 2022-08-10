using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Core.Extensions;
using Outsourcing.Core.Framework.Controllers;
using Outsourcing.Data.Models;
using Outsourcing.Service;

namespace Labixa.Areas.Admin.Controllers
{
    //[Authorize(Roles = ("Admin"))]
    public class StoreController : BaseController
    {
        #region Field

        readonly IStoreService _StoreService;
        readonly IBrandService _BrandService;
        readonly IWardService _wardService;
        readonly IDistrictService _districtService;
        #endregion
        public StoreController(IStoreService StoreService, IBrandService BrandService, IWardService WardService, IDistrictService districtService)
        {
            _StoreService = StoreService;
            _BrandService = BrandService;
            _wardService = WardService;
            _districtService = districtService;
        }
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
        public ActionResult Index()
        {
            GetCookie();
            var list = _StoreService.GetStores().Where(p=>p.BrandId==_BRAND_ID).ToList();
            return View(model: list);
        }

        public ActionResult Create()
        {
            GetCookie();
            var listBrand = _BrandService.GetBrands().ToSelectListItems(-1);
            var listDistrict = _districtService.GetDistricts().ToSelectListItems(-1);
            var listWard = _wardService.GetWards().ToSelectListItems(-1);
            var list = new StoreFormModel { ListBrands = listBrand, ListDistricts = listDistrict,ListWards = listWard };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(StoreFormModel newStore, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                //
                newStore.DateCreated = DateTime.Now;
                var item = Mapper.Map<StoreFormModel, Store>(newStore);
                _StoreService.CreateStore(item);
                return continueEditing ? RedirectToAction("Edit", "Store", new { id = item.Id })
                                    : RedirectToAction("Index", "Store");
            }
            else
            {
                return View("Create", newStore);
            }
        }

        public ActionResult Edit(int id)
        {
            var Store = _StoreService.GetStoreById(id);
            StoreFormModel StoreFormModel = Mapper.Map<Store, StoreFormModel>(Store);
            StoreFormModel.ListBrands = _BrandService.GetBrands().ToSelectListItems(-1);
            StoreFormModel.ListWards = _wardService.GetWards().ToSelectListItems(-1);
            StoreFormModel.ListDistricts = _districtService.GetDistricts().ToSelectListItems(-1);
            return View(model: StoreFormModel);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(StoreFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                Store item = Mapper.Map<StoreFormModel, Store>(obj);
                item.DateCreated = DateTime.Now;
                _StoreService.EditStore(item);
                return continueEditing ? RedirectToAction("Edit", "Store", new { id = item.Id })
                    : RedirectToAction("Index", "Store");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _StoreService.GetStoreById(id);
            item.IsDelete = true;
            _StoreService.EditStore(item);
            return RedirectToAction("Index", "Store");
        }
    }
}

