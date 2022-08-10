using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XProject.Contract.Repository.Infrastructure;
using XProject.Contract.Repository.Interface;
using XProject.Contract.Repository.Models;
using XProject.Contract.Service;

namespace XProject.Service
{
    [ScopedDependency(ServiceType = typeof(IWorking_TimeService))]
    public class Working_TimeService : Base.Service, IWorking_TimeService
    {
        private readonly IWorking_TimeRepository _workingDailyRepo;
        private readonly IUnitOfWork _uof;
        public Working_TimeService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _workingDailyRepo = serviceProvider.GetRequiredService<IWorking_TimeRepository>();
            _uof = serviceProvider.GetRequiredService<IUnitOfWork>();
        }

        public void Create(Working_Time model)
        {
            _workingDailyRepo.Add(model);
            _uof.SaveChanges();
        }

        public Working_Time CreateTimeTracking(string Ext, string BrandId, string DailyId)
        {
            var timetracking = _workingDailyRepo.Get(p => p.Working_DailyId.Equals(DailyId) && p.WorkStart.Value.Date >= DateTime.Now.Date && p.WorkEnd == null).FirstOrDefault();
            if (timetracking == null)
            {
                timetracking = new Working_Time();
                timetracking.Status = "Online";
                timetracking.WorkStart = DateTime.Now;
                timetracking.Working_DailyId = DailyId;
                _workingDailyRepo.Add(timetracking);
                _uof.SaveChanges();
                return timetracking;
            }
            return timetracking;
        }

        public Working_Time CreateTimeTracking_Offline(string Ext, string BrandId, string DailyId)
        {
            var timetracking = _workingDailyRepo.Get(p => p.Working_DailyId.Equals(DailyId) && p.WorkStart.Value.Date >= DateTime.Now.Date && p.WorkEnd == null).FirstOrDefault();
            if (timetracking != null)
            {
                timetracking.WorkEnd = DateTime.Now;
                timetracking.Status = "Offline";
                timetracking.Duration = (int)(DateTime.Now - timetracking.WorkStart.Value).TotalSeconds;
                //tinh duration
                _workingDailyRepo.Update(timetracking);
                _uof.SaveChanges();
                return timetracking;
            }
            return null;
        }

        public void Delete(string id)
        {
            var item = _workingDailyRepo.Get(x => x.Id == id).FirstOrDefault();
            if (item == null) return;

            _workingDailyRepo.Delete(item);
            _uof.SaveChanges();
        }

        public List<Working_Time> GetAll()
        {
            return _workingDailyRepo.Get().ToList();
        }

        public Working_Time GetById(string id)
        {
            return _workingDailyRepo.Get(x => x.Id == id).FirstOrDefault();
        }

        public void Update(Working_Time model)
        {
            _workingDailyRepo.Update(model);
            _uof.SaveChanges();
        }
    }
}
