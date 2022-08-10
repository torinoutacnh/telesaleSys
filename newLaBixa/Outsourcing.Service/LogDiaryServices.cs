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
    public interface ILogdiaryService
    {
        IEnumerable<Logdiary> GetAvailableCategorys();

        IEnumerable<Logdiary> GetLogdiarys();
        Logdiary GetLogdiaryById(int LogdiaryId);
        void CreateLogdiary(Logdiary Logdiary);
        Logdiary CreateLogdiaryWithResponse(Logdiary LogDiary);
        void EditLogdiary(Logdiary LogdiaryToEdit);
        void DeleteLogdiary(int LogdiaryId);
        void SaveLogdiary();

        //Logdiary GetCategoryByUrlName(string );

    }
    class LogdiaryService : ILogdiaryService
    {
        #region Field
        private readonly ILogDiaryRepository LogdiaryRepository;
        private readonly IUnitOfWork unitOfWork;
        #endregion

        #region Ctor
        public LogdiaryService(ILogDiaryRepository LogdiaryRepository, IUnitOfWork unitOfWork)
        {
            this.LogdiaryRepository = LogdiaryRepository;
            this.unitOfWork = unitOfWork;
        }
        #endregion

        public IEnumerable<Logdiary> GetAvailableCategorys()
        {
            var list = LogdiaryRepository.GetAll().Where(p => p.Deleted == true);
            return list;
        }

        public IEnumerable<Logdiary> GetLogdiarys()
        {
            try
            {
                var list = LogdiaryRepository.GetAll().Where(p => p.Deleted == false);
                return list;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public Logdiary GetLogdiaryById(int LogdiaryId)
        {
            var item = LogdiaryRepository.Get(p => p.Id == LogdiaryId);
            return item;
        }

        public void CreateLogdiary(Logdiary Logdiary)
        {
            if (Logdiary != null)
            {
               
                    LogdiaryRepository.Add(Logdiary);
                    SaveLogdiary();
            }
        }
        public Logdiary CreateLogdiaryWithResponse(Logdiary Logdiary)
        {
            if (Logdiary != null)
            {
                LogdiaryRepository.Add(Logdiary);
                SaveLogdiary();
                return Logdiary;
            }
            return null;
        }

        public void EditLogdiary(Logdiary LogdiaryToEdit)
        {
            if (LogdiaryToEdit != null)
            {
                LogdiaryRepository.Update(LogdiaryToEdit);
                SaveLogdiary();
            }
        }

        public void DeleteLogdiary(int LogdiaryId)
        {
            var item = LogdiaryRepository.Get(p => p.Id == LogdiaryId);
            // LogdiaryRepository.Delete(item);
            item.Deleted = true;
            LogdiaryRepository.Update(item);
            SaveLogdiary();
        }

        public void SaveLogdiary()
        {
            unitOfWork.Commit();
        }
    }
}
