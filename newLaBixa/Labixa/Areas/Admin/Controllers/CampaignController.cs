using Labixa.Areas.Admin.ViewModel;
using Outsourcing.Data.Models;
using Outsourcing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Labixa.Areas.Admin.Controllers
{
    public class CampaignController : BaseController
    {
        readonly ICampaignService service;

        public CampaignController(ICampaignService service)
        {
            this.service = service;
        }
        // GET: /Admin/Campaign/
        public ActionResult Index()
        {
            var model = service.GetCampaigns();
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var mode = new CampaignFormModel();
            mode.Status = 1;
            mode.Type = 1;
            return View(mode);
        }
        [HttpPost]
        public ActionResult Create(Campaign campaign)
        {

            service.CreateCampaign(campaign);
            var campaigns = service.GetCampaigns();
            return View("Index", campaigns);
        }
        [Authorize(Roles = ("Admin"))]
        public ActionResult Delete(int Id)
        {

            var campaign = service.GetCampaignById(Id);
            if (campaign != null)
            {
                //level.Deleted = true;
                service.DeleteCampaign(campaign.Id);
            }
            var campaigns = service.GetCampaigns();
            return View("Index", campaigns);
        }

        public ActionResult Update(int Id)
        {
            var campaign = service.GetCampaignById(Id);
            //schedule.IsActive = true;
            return View(campaign);
        }

        [HttpPost]
        public ActionResult Update(Campaign campaign)
        {
            service.EditCampaign(campaign);
            return View(campaign);
        }
    }
}