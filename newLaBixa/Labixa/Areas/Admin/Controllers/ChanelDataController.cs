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
    public class ChanelDataController : BaseController
    {
        #region Field

        readonly IChanelDataService _ChanelDataService;
        readonly ISourceDataService _SourceDataService;

        #endregion
        public ChanelDataController(IChanelDataService ChanelDataService, ISourceDataService SourceDataService)
        {
            _ChanelDataService = ChanelDataService;
            _SourceDataService = SourceDataService;
        }

        public ActionResult Index()
        {

            var list = _ChanelDataService.GetChanelDatas().ToList();
            return View(model: list);
        }

        public ActionResult Create()
        {
            var listSourceData = _SourceDataService.GetSourceDatas().ToSelectListItems(-1);
            var list = new ChanelDataFormModel { ListSource = listSourceData };
            return View(list);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(ChanelDataFormModel newChanelData, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                //
                newChanelData.DateCreated = DateTime.Now;
                var item = Mapper.Map<ChanelDataFormModel, ChanelData>(newChanelData);
                _ChanelDataService.CreateChanelData(item);
                return continueEditing ? RedirectToAction("Edit", "ChanelData", new { id = item.Id })
                                    : RedirectToAction("Index", "ChanelData");
            }
            else
            {
                return View("Create", newChanelData);
            }
        }

        public ActionResult Edit(int id)
        {
            var ChanelData = _ChanelDataService.GetChanelDataById(id);
            ChanelDataFormModel ChanelDataFormModel = Mapper.Map<ChanelData, ChanelDataFormModel>(ChanelData);
            ChanelDataFormModel.ListSource = _SourceDataService.GetSourceDatas().ToSelectListItems(-1);
            return View(model: ChanelDataFormModel);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(ChanelDataFormModel obj, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                ChanelData item = Mapper.Map<ChanelDataFormModel, ChanelData>(obj);
                item.DateCreated = DateTime.Now;
                _ChanelDataService.EditChanelData(item);
                return continueEditing ? RedirectToAction("Edit", "ChanelData", new { id = item.Id })
                    : RedirectToAction("Index", "ChanelData");
            }
            else
                return View("Edit", obj);
        }
        public ActionResult Delete(int id)
        {
            var item = _ChanelDataService.GetChanelDataById(id);
            item.Deleted = true;
            _ChanelDataService.EditChanelData(item);
            return RedirectToAction("Index", "ChanelData");
        }
    }
}

