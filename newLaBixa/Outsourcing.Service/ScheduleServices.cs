using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outsourcing.Core.Common;
using Outsourcing.Data.Models;
using Outsourcing.Data.Infrastructure;
using Outsourcing.Data.Repository;
using Outsourcing.Service.Properties;

namespace Outsourcing.Service
{

    public interface IScheduleService
    {
        IEnumerable<Schedule> GetAvailableCategorys();

        IEnumerable<Schedule> GetSchedules();
        void CreateSchedule(Schedule Schedule);
        void EditSchedule(Schedule ScheduleToEdit);
        void DeleteSchedule(int ScheduleId);
        void SaveSchedule();
        Schedule GetScheduleById(int ScheduleId);
        Schedule GetLastScheduleByCode(string ScheduleCode);
        //Schedule GetCategoryByUrlName(string );

    }
    class ScheduleService : IScheduleService
    {
        #region Field
        private readonly IScheduleRepository ScheduleRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public ScheduleService(IScheduleRepository ScheduleRepository, IUnitOfWork unitOfWork)
        {
            this.ScheduleRepository = ScheduleRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<Schedule> GetAvailableCategorys()
        {
            var list = ScheduleRepository.GetAll().Where(p => p.IsDeleted == true);
            return list;
        }

        public IEnumerable<Schedule> GetSchedules()
        {
            var Schedules = ScheduleRepository.GetMany(n => n.IsDeleted == false).OrderByDescending(b => b.CreateDate);
            return Schedules;
        }

        public Schedule GetScheduleById(int ScheduleId)
        {
            var Schedule = ScheduleRepository.GetById(ScheduleId);
            return Schedule;
        }
        public void EditSchedule(Schedule ScheduleToEdit)
        {
            ScheduleToEdit.CreateDate = DateTime.Now;
            ScheduleToEdit.EditDate = DateTime.Now;
            ScheduleRepository.Update(ScheduleToEdit);
            SaveSchedule();
        }


        public void CreateSchedule(Schedule Schedule)
        {
            Schedule.CreateDate = DateTime.Now;
            Schedule.EditDate = DateTime.Now;

            ScheduleRepository.Add(Schedule);
            SaveSchedule();
        }
        public Schedule GetLastScheduleByCode(string ScheduleCode)
        {
            var entity = ScheduleRepository.GetAll().Where(s => s.Code == ScheduleCode).OrderBy(s => s.CreateDate).LastOrDefault();
            if (entity != null)
            {
                return entity;
            }
            return null;
        }


        public void DeleteSchedule(int ScheduleId)
        {
            //Get Schedule by id.
            var Schedule = ScheduleRepository.GetById(ScheduleId);
            Schedule.IsDeleted = true;
            if (Schedule != null)
            {

                ScheduleRepository.Update(Schedule);
                SaveSchedule();
            }
        }

        public void SaveSchedule()
        {
            unitOfWork.Commit();
        }
    }
}
