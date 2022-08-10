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
    public class WardController : BaseController
    {
        readonly IWardService _WardService;


        public WardController(IWardService WardService)
        {
            _WardService = WardService;


        }

        public ActionResult Index()
        {
            var list = _WardService.GetWards().ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var listWard = _WardService.GetWards().ToSelectListItems(-1);
            var list = new WardFormModel { ward = listWard };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(WardFormModel newWard, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
      

                var item = Mapper.Map<WardFormModel, Ward>(newWard);
                _WardService.CreateWard(item);
                return continueEditing ? RedirectToAction("Edit", "Ward", new { id = item.Id })
                                    : RedirectToAction("Index", "Ward");
            }
            else
            {
                return View("Create", newWard);
            }
        }

        public ActionResult Edit(int id)
        {
            var item = _WardService.GetWardById(id);


            WardFormModel obj = Mapper.Map<Ward, WardFormModel>(item);
            if (obj != null)
            {
                return View(obj);
            }
            return RedirectToAction("Index", "Ward");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(WardFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                Ward item = Mapper.Map<WardFormModel, Ward>(obj);
                _WardService.EditWard(item);
                return continueEditing ? RedirectToAction("Edit", "Ward", new { id = item.Id })
                    : RedirectToAction("Index", "Ward");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _WardService.GetWardById(id);
            item.IsDeleted = true;
            _WardService.EditWard(item);
            return RedirectToAction("Index", "Ward");
        }
    }
}

