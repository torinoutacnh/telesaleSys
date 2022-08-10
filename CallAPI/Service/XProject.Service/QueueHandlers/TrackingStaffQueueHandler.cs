using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Service;
using XProject.Core.Models.QueueMessage;

namespace XProject.Service.QueueHandlers
{
    public class TrackingStaffQueueHandler
    {
        private readonly IWorking_DailyService _Working_DailyService;
        private readonly IWorking_TimeService _Working_TimeService;
        private readonly ILogger _logger;

        public TrackingStaffQueueHandler(IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;

            _Working_DailyService = serviceProvider.GetRequiredService<IWorking_DailyService>();
            _Working_TimeService = serviceProvider.GetRequiredService<IWorking_TimeService>();

        }

        public void ProcessMessage(TrackingStaffQueueMessage msg)
        {
            _logger.Information(JsonConvert.SerializeObject(msg)+" ------");
            if (msg.Status.ToLower().Equals("online"))
            {
                //online tracking
                var daily = _Working_DailyService.CreateTracking(msg.Ext, msg.BrandId);
                var time = _Working_TimeService.CreateTimeTracking(msg.Ext, msg.BrandId, daily.Id);
            }
            else
            {
                //offline tracking
                var daily = _Working_DailyService.CreateTracking_Offline(msg.Ext, msg.BrandId);
                if (daily != null)
                {
                    var time = _Working_TimeService.CreateTimeTracking_Offline(msg.Ext, msg.BrandId, daily.Id);
                }
            }
        }
    }
}
