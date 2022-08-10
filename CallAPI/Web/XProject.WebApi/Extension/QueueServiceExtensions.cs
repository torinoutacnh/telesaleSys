using Invedia.Queue.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using XProject.Core.Configs;
using XProject.Core.Models.QueueMessage;
using XProject.Service.QueueHandlers;

namespace XProject.WebApi.Extension
{
    public static class QueueServiceExtensions
    {
        public static IApplicationBuilder UseQueueServices(this IApplicationBuilder app)
        {
            var queueService = app.ApplicationServices.GetRequiredService<IQueueService>();

            if (QueueSetting.Instance.TrackingStaff.MaxWorker > 0)
            {
                var actionTransLog = QueueSetting.Instance.TrackingStaff;
                queueService.CreateConsumer<TrackingStaffQueueMessage>(msg =>
                {
                    var handler = app.ApplicationServices.GetRequiredService<TrackingStaffQueueHandler>();
                    handler.ProcessMessage(msg);
                },
                    new[] { actionTransLog.QueueTopic }, actionTransLog.QueueName, actionTransLog.MaxWorker
                );
            }

            return app;
        }
    }
}