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
    [Authorize(Roles = ("Admin"))]
    public class SourceDataController : BaseController
    {
        #region Field

        readonly ISourceDataService _SourceDataService;

        #endregion
        public SourceDataController(ISourceDataService SourceDataService)
        {
            _SourceDataService = SourceDataService;


        }

        public ActionResult Index()
        {
            var list = _SourceDataService.GetSourceDatas().ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            var listSourceData = _SourceDataService.GetSourceDatas().ToSelectListItems(-1);
            var list = new SourceDataFormModel { SourceData = listSourceData };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(SourceDataFormModel newSourceData, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                newSourceData.DateCreated = DateTime.Now;

                var item = Mapper.Map<SourceDataFormModel, SourceData>(newSourceData);
                _SourceDataService.CreateSourceData(item);
                return continueEditing ? RedirectToAction("Edit", "SourceData", new { id = item.Id })
                                    : RedirectToAction("Index", "SourceData");
            }
            else
            {
                return View("Create", newSourceData);
            }
        }

        public ActionResult Edit(int id)
        {
            var item = _SourceDataService.GetSourceDataById(id);


            SourceDataFormModel obj = Mapper.Map<SourceData, SourceDataFormModel>(item);
            if (obj != null)
            {
                return View(obj);
            }
            return RedirectToAction("Index", "SourceData");
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(SourceDataFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                SourceData item = Mapper.Map<SourceDataFormModel, SourceData>(obj);
                _SourceDataService.EditSourceData(item);
                return continueEditing ? RedirectToAction("Edit", "SourceData", new { id = item.Id })
                    : RedirectToAction("Index", "SourceData");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _SourceDataService.GetSourceDataById(id);
            item.Deleted = true;
            _SourceDataService.EditSourceData(item);
            return RedirectToAction("Index", "SourceData");
        }
    }
}

