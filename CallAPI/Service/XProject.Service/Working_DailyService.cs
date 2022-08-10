using Invedia.DI.Attributes;
using Invedia.Queue.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service;
using XProject.Core.Configs;
using XProject.Core.Models.QueueMessage;
using XProject.Core.Models.Tracking;

namespace XProject.Service
{
    [ScopedDependency(ServiceType = typeof(IWorking_DailyService))]
    public class Working_DailyService : Base.Service, IWorking_DailyService
    {
        private readonly ILogger _logger;

        private readonly IWorking_DailyRepository _workingDailyRepo;
        private readonly IWorking_TimeRepository _workingTimeRepo;
        private readonly IUnitOfWork _uof;
        private readonly IQueueService _queueService;

        public Working_DailyService(IServiceProvider serviceProvider, IWorking_TimeRepository workingTimeRepo) : base(serviceProvider)
        {
            _workingDailyRepo = serviceProvider.GetRequiredService<IWorking_DailyRepository>();
            _workingTimeRepo = serviceProvider.GetRequiredService<IWorking_TimeRepository>();
            _uof = serviceProvider.GetRequiredService<IUnitOfWork>();
            _queueService = serviceProvider.GetRequiredService<IQueueService>();
            _logger = Log.Logger;

        }

        public void Create(Working_Daily model)
        {
            _workingDailyRepo.Add(model);
            _uof.SaveChanges();
        }

        public Working_Daily CreateTracking(string Ext, string BrandId)
        {
            _logger.Information(DateTime.Today.ToString());
            var tracking = _workingDailyRepo.Get(p => p.Brand_ID.Equals(BrandId) && p.Ext.Equals(Ext) && p.DateStart.Value.Date >= DateTime.Today).FirstOrDefault();
            if (tracking == null)
            {
                tracking = new Working_Daily();
                tracking.Brand_ID = BrandId;
                tracking.Ext = Ext;
                tracking.DateStart = DateTime.Now;
                tracking.Status = "Online";
                _workingDailyRepo.Add(tracking);
                _uof.SaveChanges();
            }
            tracking.Status = "Online";
            _workingDailyRepo.Update(tracking);
            _uof.SaveChanges();
            return tracking;
        }

        public Working_Daily CreateTracking_Offline(string Ext, string BrandId)
        {
            var tracking = _workingDailyRepo.Get(p => p.Brand_ID.Equals(BrandId) && p.Ext.Equals(Ext) && p.DateStart.Value.Date >= DateTime.Now.Date).FirstOrDefault();
            if (tracking == null)
            {
                tracking = new Working_Daily();
                tracking.Brand_ID = BrandId;
                tracking.Ext = Ext;
                tracking.DateStart = DateTime.Now;
                tracking.Status = "Offline";
                _workingDailyRepo.Add(tracking);
                _uof.SaveChanges();
            }
            return tracking;
        }

        public void Delete(string id)
        {
            var item = _workingDailyRepo.Get(x => x.Id == id).FirstOrDefault();
            if (item == null) return;

            _workingDailyRepo.Delete(item);
            _uof.SaveChanges();
        }

        public List<Working_Daily> GetAll()
        {
            return _workingDailyRepo.Get().ToList();
        }

        public Working_Daily GetById(string id)
        {
            return _workingDailyRepo.Get(x => x.Id == id).FirstOrDefault();
        }

        public void PushQueue(string message = "")
        {
            _logger.Information("push queue services");
            _logger.Information(message);
            if (message == "")
            {
                message = "[{\"Name\":\"\",\"Ext\":\"101\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"102\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"103\",\"Status\":\"\"},{\"Name\":\"admin\",\"Ext\":\"104\",\"Status\":\"Online\"},{\"Name\":\"\",\"Ext\":\"105\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"106\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"107\",\"Status\":\"\"},{\"Name\":\"\",\"Ext\":\"108\",\"Status\":\"\"}]";

            }
            var myDeserializedClass = JsonConvert.DeserializeObject<List<Staff>>(message);
            foreach (var item in myDeserializedClass)
            {
                var msg = new TrackingStaffQueueMessage
                {
                    BrandId = item.BrandId == "" ? "0" : item.BrandId,
                    Ext = item.Ext,
                    Name = item.Name,
                    Status = item.Status == "" ? "offline" : "online"
                };
            var queueTopic = QueueSetting.Instance.TrackingStaff.QueueTopic;
            var queueName = QueueSetting.Instance.TrackingStaff.QueueName;
            _queueService.PublishMessage(msg, queueName, queueTopic);
            }

        }

        public void Update(Working_Daily model)
        {
            _workingDailyRepo.Update(model);
            _uof.SaveChanges();
        }

        /*
        If from & to are null => get today tracking
        If ext is null => get all ext tracking
        */
        public List<TrackingDetailModel> GetTrackingDetails(TrackingRequest model)
        {
            var results = new List<TrackingDetailModel>();

            if (model.from == null) model.from = DateTime.Today;
            if (model.to == null) model.to = DateTime.UtcNow;

            var daily = new List<Working_Daily>();
            if (model.ext == null)
            {
                daily = _workingDailyRepo.Get(x => x.Brand_ID == model.brandid && x.CreatedTime >= model.from && x.CreatedTime <= model.to).ToList();
            }
            else
            {
                daily = _workingDailyRepo.Get(x => x.Brand_ID == model.brandid && x.Ext == model.ext && x.CreatedTime >= model.from && x.CreatedTime <= model.to).ToList();
            }

            foreach (var item in daily)
            {
                var times = _workingTimeRepo.Get(x => x.Working_DailyId == item.Id).OrderByDescending(x => x.CreatedTime);
                var latest = times.FirstOrDefault();

                if (latest != null)
                {
                    var nullWorkEnd = times.Where(x => x.WorkEnd == null).OrderByDescending(x => x.CreatedTime);
                    if (nullWorkEnd.Count() > 1)
                    {
                        continue;
                    }
                    var we = nullWorkEnd.FirstOrDefault();
                    if (we != null && we.CreatedTime < latest.CreatedTime)
                    {
                        continue;
                    }
                }

                var result = new TrackingDetailModel()
                {
                    BrandId = item.Brand_ID,
                    Ext = item.Ext,
                    Duration = times.Sum(t => t.Duration),
                };
                results.Add(result);
            }

            return results;
        }
    }

    public class Staff
    {
        public string Name { get; set; }
        public string Ext { get; set; }
        public string Status { get; set; }
        public string BrandId { get; set; }
    }
}
