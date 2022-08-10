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
    public class OATemplateController : BaseController
    {
        readonly IOATemplateService _OATemplateService;
        readonly IOATemplateAttributeService _OATemplateAttributeService;


        public OATemplateController(IOATemplateService OATemplateServic, IOATemplateAttributeService OATemplateAttributeService)
        {
            _OATemplateService = OATemplateServic;
            _OATemplateAttributeService = OATemplateAttributeService;
        }
        //
        // GET: /Admin/OATemplate/
        public ActionResult Index()
        {
            var listOATemplate = _OATemplateService.GetOATemplatesByBrandId(paramet._BRAND_ID);

            return View(listOATemplate);
        }

        public ActionResult Create()
        {
            var OAtemplatemodel = new OATemplateModel ();
            return View(OAtemplatemodel);
        }
        [ValidateInput(false)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(OATemplateModel newOATemplate, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var item = Mapper.Map<OATemplateModel,OATemplate>(newOATemplate);
                _OATemplateService.CreateOATemplate(item);
                return continueEditing ? RedirectToAction("Edit", "OATemplate", new { id = item.Id })
                                    : RedirectToAction("Index", "OATemplate");
            }
            else
            {
                return View("Create", newOATemplate);
            }
        }
    }
}