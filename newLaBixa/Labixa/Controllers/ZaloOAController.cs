﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Controllers
{
    public class ZaloOAController : Controller
    {
        //
        // GET: /ZaloOA/
        public ActionResult Index()
        {
            return View();
        }
        public  ActionResult SendTemplate()
        {
            return RedirectToAction("Index");
        }
	}

}