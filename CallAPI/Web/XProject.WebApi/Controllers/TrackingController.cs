using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using XProject.Contract.Service;
using XProject.Core.Constants;
using XProject.Core.Models.Tracking;

namespace XProject.WebApi.Controllers
{
    public class TrackingController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWorking_DailyService _working_DailyService;
        private readonly IWorking_TimeService _working_TimeService;

        public TrackingController(IWorking_TimeService working_TimeService, IWorking_DailyService working_DailyService)
        {
            _logger = Log.Logger;
            _working_DailyService = working_DailyService;
            _working_TimeService = working_TimeService;
        }

        [Route(Endpoints.TrackingEndpoint.GetByBrand)]
        [HttpPost]
        public List<TrackingDetailModel> GetByBrand([FromBody]TrackingRequest model)
        {
            return _working_DailyService.GetTrackingDetails(model);
        }
    }
}
