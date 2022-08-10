using Autofac;
using AutoMapper;
using System;
using Invedia.Core.ConfigUtils;
using Invedia.Queue.RabbitMq;
using XProject.Core.Utils;
using XProject.Service.QueueHandlers;

namespace XProject.WebApi.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var mq = SystemHelper.Configs.GetSection<RabbitMqSettings>("RabbitMqSettings");
            builder.Register(c => new RabbitMqService(mq)).As<IQueueService>().SingleInstance();
            RegisterQueue(builder);
        }

        private static void RegisterQueue(ContainerBuilder builder)
        {
            builder.Register(c => new TrackingStaffQueueHandler(c.Resolve<IServiceProvider>())).InstancePerDependency();
            //builder.Register(c => new CrawllerQueueHandlers(c.Resolve<IServiceProvider>())).InstancePerDependency();
            //builder.Register(c => new TeamBetQueueHandlers(c.Resolve<IServiceProvider>())).InstancePerDependency();
            //builder.Register(c => new WithdrawQueueHandlers(c.Resolve<IServiceProvider>())).InstancePerDependency();
        }
    }
}
